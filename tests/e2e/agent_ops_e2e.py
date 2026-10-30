"""
Browser E2E for the Agent Ops page (/agent-ops) with screenshots and an axe-core audit.

Start the web app first, then:

    npm install
    dotnet run --project src/Net11Showcase.Web/Net11Showcase.Web --urls http://127.0.0.1:5290 --no-launch-profile
    python tests/e2e/agent_ops_e2e.py http://127.0.0.1:5290
"""
import json
import os
import sys

from playwright.sync_api import expect, sync_playwright

BASE = sys.argv[1] if len(sys.argv) > 1 else "http://127.0.0.1:5290"
ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
SHOTS = os.path.join(ROOT, "docs", "images")
AXE = os.path.join(ROOT, "node_modules", "axe-core", "axe.min.js")
os.makedirs(SHOTS, exist_ok=True)

problems = []


def check(condition, message):
    print(("PASS " if condition else "FAIL ") + message)
    if not condition:
        problems.append(message)


def shot(page, name):
    path = os.path.join(SHOTS, f"net11-showcase-agent-ops-{name}.png")
    page.locator("[data-testid=agent-console]").screenshot(path=path)
    print("     screenshot:", path)


def state_value(page, key):
    row = page.locator(f"[data-testid=state-row][data-key='{key}']")
    row.wait_for(timeout=15000)
    field = row.locator("input")
    return field.input_value() if field.count() else row.locator("[data-testid=state-value]").inner_text()


def wait_status(page, text, timeout=15000):
    expect(page.locator("[data-testid=run-status]")).to_have_text(text, timeout=timeout)


def run_to_gate(page):
    page.locator("[data-testid=run]").click()
    page.locator("[data-testid=approval-gate]").wait_for(timeout=15000)


with sync_playwright() as p:
    browser = p.chromium.launch(headless=True)
    context = browser.new_context(viewport={"width": 1500, "height": 1000}, accept_downloads=True)
    page = context.new_page()
    errors = []
    page.on("console", lambda m: errors.append(m.text) if m.type == "error" else None)
    page.on("pageerror", lambda e: errors.append(str(e)))
    page.on("response", lambda r: errors.append(f"{r.status} {r.url}") if r.status >= 400 else None)

    page.goto(BASE + "/agent-ops", wait_until="networkidle", timeout=60000)
    page.locator("[data-testid=agent-console]").wait_for(timeout=20000)
    # The Blazor Server circuit must be connected before clicks do anything.
    page.wait_for_function("window.Blazor !== undefined", timeout=20000)
    page.wait_for_timeout(1500)
    shot(page, "1-idle")

    # 1. High-risk call blocks until approved; edited arguments are what actually executes.
    run_to_gate(page)
    wait_status(page, "Waiting for your approval")
    check(state_value(page, "refund.status") == "proposed", "refund is only 'proposed' while the gate is open")
    check(page.locator("[data-testid=tool-call][data-tool=send_email]").count() == 0, "follow-up action has not run while blocked")
    shot(page, "2-approval-gate")
    page.locator("textarea[id^=gate-args-]").fill('{"orderId":"4821","amount":"120.00"}')
    page.locator("[data-testid=approve]").click()
    wait_status(page, "Finished")
    check(state_value(page, "refund.status") == "issued", "approved: refund issued")
    check(state_value(page, "refund.amount") == "120.00", "approved with edited arguments: $120.00 was executed")
    shot(page, "3-approved")

    # 2. Rejection reroutes the agent.
    run_to_gate(page)
    page.locator("input[id^=gate-reason-]").fill("over policy limit")
    page.locator("[data-testid=reject]").click()
    wait_status(page, "Finished")
    check(state_value(page, "refund.status") == "blocked", "rejected: refund stays blocked")
    check(page.locator("[data-testid=tool-call][data-tool=send_email]").count() == 0, "rejected: no customer email sent")
    check(page.locator("[data-testid=tool-call][data-tool=create_ticket]").count() == 1, "rejected: agent escalated with a ticket")
    shot(page, "4-rejected")

    # 3. Audit export downloads the JSON trail (via the component library's JS module).
    with page.expect_download() as dl:
        page.locator("[data-testid=audit-export]").click()
    data = json.load(open(dl.value.path(), encoding="utf-8"))
    check(any(e["action"] == "tool.reject" and "over policy limit" in e["detail"] for e in data), "exported JSON has the rejection and its reason")

    # 4. Takeover pauses the agent; the human's edit changes its next tool call.
    page.locator("[data-testid=run]").click()
    # Wait for the NEW run (Run disabled) and its lookup step; the previous run's rows are still on screen until then.
    expect(page.locator("[data-testid=run]")).to_be_disabled()
    page.locator("[data-testid=tool-call][data-tool=lookup_order][data-status=Completed]").wait_for(timeout=15000)
    page.locator("[data-testid=takeover]").click()
    wait_status(page, "Paused: you have control")
    before = page.locator("[data-testid=tool-call]").count()
    page.wait_for_timeout(1500)
    check(page.locator("[data-testid=tool-call]").count() == before, "paused: the agent did nothing while the human had control")
    shot(page, "5-takeover")
    box = page.locator("[data-testid=state-row][data-key='refund.amount'] input")
    box.fill("75.00")
    box.press("Tab")
    page.locator("[data-testid=handback]").click()
    page.locator("[data-testid=approval-gate]").wait_for(timeout=15000)
    args = page.locator("textarea[id^=gate-args-]").input_value()
    check("75.00" in args, f"human edit ($75.00) flowed into the agent's next tool call (args: {args})")

    # 5. RTL + accessibility audit at the open gate.
    page.evaluate("document.documentElement.setAttribute('dir','rtl')")
    shot(page, "6-rtl")
    page.evaluate("document.documentElement.removeAttribute('dir')")
    page.add_script_tag(path=AXE)
    violations = page.evaluate(
        """async () => {
            const r = await axe.run('[data-testid=agent-console]', { runOnly: ['wcag2a', 'wcag2aa', 'wcag21aa', 'wcag22aa'] });
            return r.violations.map(v => ({ id: v.id, impact: v.impact, help: v.help, nodes: v.nodes.slice(0, 3).map(n => n.target.join(' ')) }));
        }"""
    )
    for v in violations:
        print("     axe:", v["impact"], v["id"], "-", v["help"], v["nodes"])
    check(len(violations) == 0, f"axe-core: no WCAG 2.x A/AA violations in the agent console ({len(violations)} found)")

    page.locator("[data-testid=cancel]").click()
    real = [e for e in errors if "favicon" not in e.lower() and not e.startswith("Failed to load resource")]
    check(len(real) == 0, f"no console errors or failed requests ({real[:3]})")
    browser.close()

print()
if problems:
    print(f"{len(problems)} FAILED:")
    for m in problems:
        print("  -", m)
    sys.exit(1)
print("ALL CHECKS PASSED")
