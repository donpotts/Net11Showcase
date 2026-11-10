// Showcase Interactive Helpers: Pixel-Perfect Canvas Signature, PDF Flattening & Aerospace Audio FX

window.contractPad = {
    canvas: null,
    ctx: null,
    isDrawing: false,
    hasInk: false,
    dotNetRef: null,

    init: function (canvasElement, dotNetHelper) {
        this.canvas = canvasElement;
        this.dotNetRef = dotNetHelper;
        if (!this.canvas) return;

        this.ctx = this.canvas.getContext('2d');
        this.resize();

        window.addEventListener('resize', () => this.resize());

        this.canvas.addEventListener('pointerdown', (e) => this.onPointerDown(e));
        this.canvas.addEventListener('pointermove', (e) => this.onPointerMove(e));
        this.canvas.addEventListener('pointerup', (e) => this.onPointerUp(e));
        this.canvas.addEventListener('pointercancel', (e) => this.onPointerUp(e));
    },

    resize: function () {
        if (!this.canvas) return;
        const rect = this.canvas.getBoundingClientRect();
        if (rect.width === 0 || rect.height === 0) return;
        const dpr = window.devicePixelRatio || 1;

        const temp = document.createElement('canvas');
        temp.width = this.canvas.width;
        temp.height = this.canvas.height;
        const tempCtx = temp.getContext('2d');
        if (this.canvas.width > 0 && this.canvas.height > 0) {
            tempCtx.drawImage(this.canvas, 0, 0);
        }

        this.canvas.width = rect.width * dpr;
        this.canvas.height = rect.height * dpr;
        this.ctx.scale(dpr, dpr);
        this.ctx.lineCap = 'round';
        this.ctx.lineJoin = 'round';
        this.ctx.strokeStyle = '#4338ca'; // indigo-700
        this.ctx.lineWidth = 3.2;

        if (this.hasInk && temp.width > 0) {
            this.ctx.drawImage(temp, 0, 0, rect.width, rect.height);
        }
    },

    getPos: function (e) {
        const rect = this.canvas.getBoundingClientRect();
        return {
            x: e.clientX - rect.left,
            y: e.clientY - rect.top
        };
    },

    onPointerDown: function (e) {
        if (this.canvas.dataset.sealed === 'true') return;
        this.canvas.setPointerCapture(e.pointerId);
        this.isDrawing = true;
        this.hasInk = true;
        const p = this.getPos(e);
        this.ctx.beginPath();
        this.ctx.moveTo(p.x, p.y);
        this.ctx.lineTo(p.x + 0.1, p.y + 0.1);
        this.ctx.stroke();
    },

    onPointerMove: function (e) {
        if (!this.isDrawing || this.canvas.dataset.sealed === 'true') return;
        const p = this.getPos(e);
        this.ctx.lineTo(p.x, p.y);
        this.ctx.stroke();
    },

    onPointerUp: function (e) {
        if (!this.isDrawing) return;
        this.isDrawing = false;
        try {
            this.canvas.releasePointerCapture(e.pointerId);
        } catch (_) {}

        if (this.dotNetRef) {
            const dataUrl = this.canvas.toDataURL('image/png');
            this.dotNetRef.invokeMethodAsync('OnStrokeDrawn', dataUrl);
        }
    },

    clear: function () {
        if (!this.canvas) return;
        this.hasInk = false;
        const rect = this.canvas.getBoundingClientRect();
        this.ctx.clearRect(0, 0, rect.width, rect.height);
    },

    renderPreset: function (name, roleTitle) {
        if (!this.canvas) return;
        this.hasInk = true;
        const rect = this.canvas.getBoundingClientRect();
        this.ctx.clearRect(0, 0, rect.width, rect.height);

        this.ctx.save();
        // Decorative flourish curve
        this.ctx.strokeStyle = '#818cf8';
        this.ctx.lineWidth = 2;
        this.ctx.beginPath();
        this.ctx.moveTo(rect.width * 0.15, rect.height * 0.72);
        this.ctx.bezierCurveTo(rect.width * 0.3, rect.height * 0.88, rect.width * 0.7, rect.height * 0.88, rect.width * 0.85, rect.height * 0.72);
        this.ctx.stroke();

        // Main signature cursive
        this.ctx.font = 'italic bold 32px "Segoe Script", "Brush Script MT", "Dancing Script", cursive, Georgia, serif';
        this.ctx.fillStyle = '#3730a3';
        this.ctx.textAlign = 'center';
        this.ctx.textBaseline = 'middle';
        this.ctx.fillText(name, rect.width / 2, rect.height / 2 - 8);

        // Subtitle badge
        this.ctx.font = 'bold 11px ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, monospace';
        this.ctx.fillStyle = '#059669';
        this.ctx.fillText(roleTitle, rect.width / 2, rect.height / 2 + 24);

        this.ctx.restore();

        if (this.dotNetRef) {
            const dataUrl = this.canvas.toDataURL('image/png');
            this.dotNetRef.invokeMethodAsync('OnStrokeDrawn', dataUrl);
        }
    }
};

// Real Browser Flattened PDF File Download
window.downloadContractPdf = function (data) {
    const filename = `Enterprise_Contract_${data.documentId || 'MSA-2026'}.pdf`;

    const title = "ENTERPRISE SERVICE LEVEL AGREEMENT & CONTRACT";
    const docId = `Document Ref: ${data.documentId || 'MSA-2026-NET11'} | Status: CRYPTOGRAPHICALLY SEALED`;
    const signer = `Signatory: ${data.signerName} (${data.companyName})`;
    const email = `Signatory Email: ${data.email}`;
    const sla = `SLA Tier: ${data.slaTier} (99.999% Guaranteed Availability)`;
    const budget = `Budget Allocation: $${Number(data.budget || 250000).toLocaleString()}/month`;
    const hash = `SHA-256 Seal Digest: ${data.digest}`;
    const date = `Sealed At: ${new Date().toUTCString()}`;
    const footer = "Secured by .NET 11 Enterprise Platform - NIST FIPS Compliant";

    const content = [
        "BT",
        "/F1 16 Tf",
        "50 720 Td",
        `(${title}) Tj`,
        "/F1 9 Tf",
        "0 -20 Td",
        `(${docId}) Tj`,
        "0 -30 Td",
        "/F1 11 Tf",
        "(1. PARTIES & RECITALS) Tj",
        "/F1 9 Tf",
        "0 -16 Td",
        `(${signer}) Tj`,
        "0 -14 Td",
        `(${email}) Tj`,
        "0 -24 Td",
        "/F1 11 Tf",
        "(2. TERMS & SERVICE LEVEL AGREEMENT) Tj",
        "/F1 9 Tf",
        "0 -16 Td",
        `(${sla}) Tj`,
        "0 -14 Td",
        `(${budget}) Tj`,
        "0 -24 Td",
        "/F1 11 Tf",
        "(3. CRYPTOGRAPHIC VERIFICATION & TAMPER-EVIDENT SEAL) Tj",
        "/F1 9 Tf",
        "0 -16 Td",
        `(${hash}) Tj`,
        "0 -14 Td",
        `(${date}) Tj`,
        "0 -36 Td",
        "/F1 13 Tf",
        `([ SIGNED AND SEALED: ${data.signerName} ]) Tj`,
        "0 -40 Td",
        "/F1 8 Tf",
        `(${footer}) Tj`,
        "ET"
    ].join("\n");

    const streamLength = new TextEncoder().encode(content).length;

    const obj1 = "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n";
    const obj2 = "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n";
    const obj3 = "3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Contents 4 0 R /Resources << /Font << /F1 5 0 R >> >> >>\nendobj\n";
    const obj4 = `4 0 obj\n<< /Length ${streamLength} >>\nstream\n${content}\nendstream\nendobj\n`;
    const obj5 = "5 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n";

    const header = "%PDF-1.4\n";
    const pos1 = new TextEncoder().encode(header).length;
    const pos2 = pos1 + new TextEncoder().encode(obj1).length;
    const pos3 = pos2 + new TextEncoder().encode(obj2).length;
    const pos4 = pos3 + new TextEncoder().encode(obj3).length;
    const pos5 = pos4 + new TextEncoder().encode(obj4).length;
    const startXref = pos5 + new TextEncoder().encode(obj5).length;

    const pad = (n) => n.toString().padStart(10, '0');

    const xref = [
        "xref",
        "0 6",
        "0000000000 65535 f ",
        `${pad(pos1)} 00000 n `,
        `${pad(pos2)} 00000 n `,
        `${pad(pos3)} 00000 n `,
        `${pad(pos4)} 00000 n `,
        `${pad(pos5)} 00000 n `,
        "trailer",
        "<< /Size 6 /Root 1 0 R >>",
        "startxref",
        `${startXref}`,
        "%%EOF\n"
    ].join("\n");

    const pdfString = header + obj1 + obj2 + obj3 + obj4 + obj5 + xref;
    const blob = new Blob([pdfString], { type: 'application/pdf' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    setTimeout(() => {
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }, 2500);
};

// Web Audio Aerospace Sound FX Synthesizer
window.rocketAudio = {
    audioCtx: null,

    getAudio: function () {
        if (!this.audioCtx) {
            const AudioContext = window.AudioContext || window.webkitAudioContext;
            if (AudioContext) {
                this.audioCtx = new AudioContext();
            }
        }
        if (this.audioCtx && this.audioCtx.state === 'suspended') {
            this.audioCtx.resume();
        }
        return this.audioCtx;
    },

    playThrusterIgnite: function () {
        try {
            const ctx = this.getAudio();
            if (!ctx) return;
            const now = ctx.currentTime;

            const osc = ctx.createOscillator();
            const gain = ctx.createGain();
            osc.type = 'sawtooth';
            osc.frequency.setValueAtTime(70, now);
            osc.frequency.exponentialRampToValueAtTime(150, now + 0.3);
            osc.frequency.exponentialRampToValueAtTime(80, now + 0.8);

            gain.gain.setValueAtTime(0.01, now);
            gain.gain.linearRampToValueAtTime(0.18, now + 0.15);
            gain.gain.exponentialRampToValueAtTime(0.001, now + 0.8);

            osc.connect(gain);
            gain.connect(ctx.destination);
            osc.start(now);
            osc.stop(now + 0.85);
        } catch (_) {}
    },

    playAfterburnerSurge: function () {
        try {
            const ctx = this.getAudio();
            if (!ctx) return;
            const now = ctx.currentTime;

            const osc = ctx.createOscillator();
            const gain = ctx.createGain();
            osc.type = 'triangle';
            osc.frequency.setValueAtTime(110, now);
            osc.frequency.exponentialRampToValueAtTime(420, now + 0.25);
            osc.frequency.exponentialRampToValueAtTime(180, now + 1.2);

            gain.gain.setValueAtTime(0.01, now);
            gain.gain.linearRampToValueAtTime(0.28, now + 0.2);
            gain.gain.exponentialRampToValueAtTime(0.001, now + 1.2);

            osc.connect(gain);
            gain.connect(ctx.destination);
            osc.start(now);
            osc.stop(now + 1.25);
        } catch (_) {}
    },

    playShutdown: function () {
        try {
            const ctx = this.getAudio();
            if (!ctx) return;
            const now = ctx.currentTime;

            const osc = ctx.createOscillator();
            const gain = ctx.createGain();
            osc.type = 'sine';
            osc.frequency.setValueAtTime(130, now);
            osc.frequency.exponentialRampToValueAtTime(35, now + 0.6);

            gain.gain.setValueAtTime(0.14, now);
            gain.gain.exponentialRampToValueAtTime(0.001, now + 0.6);

            osc.connect(gain);
            gain.connect(ctx.destination);
            osc.start(now);
            osc.stop(now + 0.65);
        } catch (_) {}
    }
};
