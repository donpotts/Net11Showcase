import os
import subprocess
import sys
import time
import urllib.request
from playwright.sync_api import sync_playwright

BASE_DIR = r"C:\Users\donpo\.gemini\antigravity\scratch\Net11Showcase"
PROJECT_DIR = os.path.join(BASE_DIR, "src", "Net11Showcase.Web", "Net11Showcase.Web")
PORT = 5160
URL = f"http://127.0.0.1:{PORT}"
SCREENSHOT_DOCS = os.path.join(BASE_DIR, "docs", "images", "net11-showcase-suite.png")
SCREENSHOT_ARTIFACT = r"C:\Users\donpo\.gemini\antigravity\brain\27e04055-7eb7-457e-ac4c-93c7084425b7\screenshots\net11-showcase-suite.png"

def wait_for_server(url, timeout=35):
    start = time.time()
    print(f"Waiting for server at {url}...")
    while time.time() - start < timeout:
        try:
            with urllib.request.urlopen(url, timeout=1.5) as resp:
                if resp.status == 200:
                    print(f"Server is online! (Status: {resp.status})")
                    return True
        except Exception:
            pass
        time.sleep(0.5)
    return False

def run_tests():
    print(f"Starting Net11Showcase.Web on {URL}...")
    proc = subprocess.Popen(
        ["dotnet", "run", "--project", PROJECT_DIR, "--urls", URL],
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        text=True
    )

    try:
        if not wait_for_server(URL, timeout=40):
            print("ERROR: Server failed to start within timeout.")
            stdout, stderr = proc.communicate(timeout=5)
            print("STDOUT:\n", stdout)
            print("STDERR:\n", stderr)
            sys.exit(1)

        with sync_playwright() as p:
            browser = p.chromium.launch(headless=True)
            context = browser.new_context(
                viewport={"width": 1440, "height": 1100},
                device_scale_factor=2,
                color_scheme="dark"
            )
            page = context.new_page()

            print(f"Navigating to {URL}...")
            page.goto(URL, wait_until="networkidle")
            time.sleep(2)

            # Check title
            title = page.title()
            print("Page Title:", title)
            assert ".NET 11" in title

            # Check hero text
            hero = page.locator("h1").first.inner_text()
            print("Hero Heading:", hero)
            assert ".NET 11 Showcase" in hero

            # Module 1: Blazor .NET 11 interactions
            print("Testing Module 1: Blazor Rendering & QuickGrid...")
            page.locator("button:has-text('StreamingRendering')").click()
            time.sleep(0.3)
            page.locator("input[placeholder*='Filter components']").fill("Enclave")
            time.sleep(0.3)
            page.locator("tr:has-text('PQC ML-KEM')").click()
            time.sleep(0.3)

            # Module 2: MAUI Hybrid
            print("Testing Module 2: MAUI Hybrid...")
            page.locator("button:has-text('2. MAUI Hybrid Lab')").click()
            time.sleep(0.5)
            page.locator("button:has-text('Refresh')").click()
            page.locator("button:has-text('Shake')").click()
            page.locator("button:has-text('+ Enqueue Action')").click()
            time.sleep(0.5)
            maui_ss = os.path.join(BASE_DIR, "docs", "images", "net11-showcase-maui.png")
            page.screenshot(path=maui_ss)

            # Module 4: EF Core & Vector Search
            print("Testing Module 4: EF Core 11 & Vector Search...")
            page.locator("button:has-text('4. EF Core 11 & Vectors')").click()
            time.sleep(0.5)
            page.locator("input[placeholder*='semantic query']").fill("post-quantum cryptography encryption")
            page.locator("button:has-text('Vector Search')").click()
            time.sleep(0.5)

            # Module 5: Benchmarks
            print("Testing Module 5: Native AOT Benchmarks...")
            page.locator("button:has-text('5. Native AOT & Benchmarks')").click()
            time.sleep(0.5)
            bench_ss = os.path.join(BASE_DIR, "docs", "images", "net11-showcase-benchmarks.png")
            page.screenshot(path=bench_ss)

            # Module 6: AI Chat
            print("Testing Module 6: AI RAG & Prompt Studio...")
            page.locator("button:has-text('6. AI RAG & Prompt Studio')").click()
            time.sleep(0.5)
            page.locator("input[placeholder*='Ask about']").fill("How does Native AOT in .NET 11 improve memory?")
            page.locator("button:has-text('Send Prompt')").click()
            time.sleep(0.5)
            ai_ss = os.path.join(BASE_DIR, "docs", "images", "net11-showcase-ai.png")
            page.screenshot(path=ai_ss)

            # Return to overview tab for final screenshot
            page.locator("button:has-text('1. Blazor .NET 11')").click()
            time.sleep(0.5)
            page.locator("input[placeholder*='Filter components']").fill("")
            time.sleep(0.5)

            # Take high-res screenshot
            os.makedirs(os.path.dirname(SCREENSHOT_DOCS), exist_ok=True)
            os.makedirs(os.path.dirname(SCREENSHOT_ARTIFACT), exist_ok=True)

            print(f"Capturing screenshot to {SCREENSHOT_DOCS}...")
            page.screenshot(path=SCREENSHOT_DOCS, full_page=True)
            page.screenshot(path=SCREENSHOT_ARTIFACT, full_page=True)
            print("Screenshot captured successfully!")

            browser.close()
            print("Playwright E2E verification PASSED with 0 errors!")

    finally:
        print("Shutting down web host process...")
        proc.terminate()
        try:
            proc.wait(timeout=5)
        except subprocess.TimeoutExpired:
            proc.kill()
        print("Process terminated cleanly.")

if __name__ == "__main__":
    run_tests()
