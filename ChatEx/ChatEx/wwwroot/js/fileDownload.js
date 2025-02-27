function saveAsFile(filename, base64Data) {
    const link = document.createElement('a');
    link.href = `data:application/octet-stream;base64,${base64Data}`;
    link.download = filename;
    link.click();
}