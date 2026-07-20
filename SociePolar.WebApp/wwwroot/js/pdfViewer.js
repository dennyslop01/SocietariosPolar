function abrirPdfBase64(base64String, nombreArchivo) {
    const pdfUrl = "data:application/pdf;base64," + base64String;

    // Abrimos explícitamente una pestaña limpia e independiente
    const nuevaPestana = window.open("", "_blank");

    if (nuevaPestana) {
        nuevaPestana.document.write(
            `<html>
                <head>
                    <title>${nombreArchivo || "Visor de Documento"}</title>
                    <style>
                        body { margin: 0; padding: 0; overflow: hidden; background-color: #525659; }
                        embed { width: 100vw; height: 100vh; border: none; }
                    </style>
                </head>
                <body>
                    <embed src="${pdfUrl}" type="application/pdf"></embed>
                </body>
             </html>`
        );
        nuevaPestana.document.close();
    } else {
        alert("Por favor, permite las ventanas emergentes (pop-ups) para ver el PDF.");
    }
}
