// Downloads text as a file. Loaded on demand by TwAuditTrail.
export function downloadText(filename, mimeType, text) {
    const url = URL.createObjectURL(new Blob([text], { type: mimeType }));
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    a.remove();
    URL.revokeObjectURL(url);
}
