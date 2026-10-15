function abrirPdfBase64(base64String, nombreArchivo) {
    try {
        const byteCharacters = atob(base64String);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: 'application/pdf' });
        const blobUrl = URL.createObjectURL(blob);

        const nuevaPestana = window.open(blobUrl, "_blank");
        if (!nuevaPestana) {
            // Si el navegador bloqueó la ventana emergente, crear enlace de descarga de rescate
            const a = document.createElement("a");
            a.href = blobUrl;
            a.download = (nombreArchivo || "documento.pdf").replace(/[/\\?%*:|"<>]/g, '_');
            document.body.appendChild(a);
            a.click();
            setTimeout(() => a.remove(), 1000);
        }
    } catch (err) {
        console.error("Error al abrir PDF Base64:", err);
        // Fallback en caso de string muy grande o formato directo
        const pdfUrl = "data:application/pdf;base64," + base64String;
        window.open(pdfUrl, "_blank");
    }
}

function abrirExcelBase64(base64String, nombreArchivo) {
    try {
        // 1. Convertir el string Base64 a bytes binarios
        const caracteresBinarios = atob(base64String);
        const numerosBinarios = new Array(caracteresBinarios.length);

        for (let i = 0; i < caracteresBinarios.length; i++) {
            numerosBinarios[i] = caracteresBinarios.charCodeAt(i);
        }

        const matrizBytes = new Uint8Array(numerosBinarios);

        // 2. Validar el formato real analizando los "Magic Bytes" al inicio del archivo
        let tipoMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; // XLSX por defecto
        let extensionCorrecta = ".xlsx";

        if (matrizBytes.length >= 4) {
            // XLS antiguo empieza por D0 CF 11 E0 (Composite Document File V2)
            if (matrizBytes[0] === 0xD0 && matrizBytes[1] === 0xCF && matrizBytes[2] === 0x11 && matrizBytes[3] === 0xE0) {
                tipoMime = "application/vnd.ms-excel";
                extensionCorrecta = ".xls";
            }
            // XLSX moderno es un ZIP y empieza por 50 4B 03 04 (PK..)
            else if (matrizBytes[0] === 0x50 && matrizBytes[1] === 0x4B && matrizBytes[2] === 0x03 && matrizBytes[3] === 0x04) {
                tipoMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                extensionCorrecta = ".xlsx";
            }
        }

        // 3. Limpiar el nombre del archivo de extensiones previas incorrectas y poner la detectada
        let nombreLimpio = nombreArchivo.replace(/\.xlsx$/i, '').replace(/\.xls$/i, '');
        const nombreConExtension = `${nombreLimpio}${extensionCorrecta}`;

        // 4. Crear el Blob con el tipo MIME dinámico correcto
        const blobExcel = new Blob([matrizBytes], { type: tipoMime });

        // 5. Crear una URL temporal para el objeto Blob
        const urlBlob = URL.createObjectURL(blobExcel);

        // 6. Abrir en una nueva pestaña (forzará la descarga/apertura en el cliente)
        const nuevaPestana = window.open("", "_blank");

        if (nuevaPestana) {
            nuevaPestana.document.write(
                `<html>
                <head>
                    <title>${nombreConExtension}</title>
                    <style>
                        body { 
                            font-family: Arial, sans-serif; 
                            display: flex; 
                            justify-content: center; 
                            align-items: center; 
                            height: 100vh; 
                            background-color: #f4f4f9; 
                            margin: 0; 
                        }
                        .mensaje { 
                            text-align: center; 
                            padding: 20px; 
                            background: white; 
                            border-radius: 8px; 
                            box-shadow: 0 4px 6px rgba(0,0,0,0.1); 
                        }
                        a { 
                            display: inline-block; 
                            margin-top: 15px; 
                            padding: 10px 20px; 
                            background-color: #107c41; 
                            color: white; 
                            text-decoration: none; 
                            border-radius: 4px; 
                            font-weight: bold; 
                        }
                    </style>
                </head>
                <body>
                    <div class="mensaje">
                        <h2>Tu archivo Excel está listo</h2>
                        <p>Si la descarga no inició automáticamente, haz clic abajo:</p>
                        <a href="${urlBlob}" download="${nombreConExtension}">Descargar Excel</a>
                    </div>
                    <script>
                        // Disparar la descarga automática inmediatamente
                        const link = document.createElement('a');
                        link.href = "${urlBlob}";
                        link.download = "${nombreConExtension}";
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);
                    </script>
                </body>
                </html>`
            );
            nuevaPestana.document.close();
        } else {
            alert("Por favor, permite las ventanas emergentes (pop-ups) para descargar el Excel.");
        }
    } catch (error) {
        console.error("Error al procesar el archivo Excel Base64:", error);
        alert("No se pudo procesar el archivo Excel. Verifica que el formato Base64 sea correcto.");
    }
}

window.downloadFileFromStream = async (fileName, content) => {
    try {
        let cleanName = (fileName || 'archivo').replace(/[/\\?%*:|"<>]/g, '_');
        let mimeType = 'application/octet-stream';

        if (cleanName.toLowerCase().endsWith('.pdf')) {
            mimeType = 'application/pdf';
        } else if (cleanName.toLowerCase().endsWith('.csv')) {
            mimeType = 'text/csv;charset=utf-8;';
        } else if (cleanName.toLowerCase().endsWith('.xlsx') || cleanName.toLowerCase().endsWith('.xls')) {
            mimeType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
        }

        let blob;
        if (content && typeof content.arrayBuffer === 'function') {
            const arrayBuffer = await content.arrayBuffer();
            blob = new Blob([arrayBuffer], { type: mimeType });
        } else if (typeof content === 'string') {
            // Añadir BOM UTF-8 (\ufeff) para que Microsoft Excel reconozca tildes y caracteres especiales en español
            blob = new Blob(["\ufeff", content], { type: mimeType });
        } else if (content instanceof Blob) {
            blob = content;
        } else {
            blob = new Blob([content], { type: mimeType });
        }

        const url = URL.createObjectURL(blob);
        const anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = cleanName;
        document.body.appendChild(anchorElement);
        anchorElement.click();
        setTimeout(() => {
            anchorElement.remove();
            URL.revokeObjectURL(url);
        }, 1500);
    } catch (err) {
        console.error("Error al descargar archivo:", err);
    }
};

window.descargarPdfBase64 = function (base64String, nombreArchivo) {
    try {
        let cleanName = (nombreArchivo || 'Reporte.pdf').replace(/[/\\?%*:|"<>]/g, '_');
        if (!cleanName.toLowerCase().endsWith('.pdf')) {
            cleanName += '.pdf';
        }

        const byteCharacters = atob(base64String);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: 'application/pdf' });
        const url = URL.createObjectURL(blob);
        const anchor = document.createElement('a');
        anchor.href = url;
        anchor.download = cleanName;
        document.body.appendChild(anchor);
        anchor.click();
        setTimeout(() => {
            anchor.remove();
            URL.revokeObjectURL(url);
        }, 30000);
    } catch (err) {
        console.error("Error al descargar PDF Base64:", err);
    }
};

window.descargarYAbrirPdf = function (base64String, nombreArchivo, relativeUrl) {
    try {
        let cleanName = (nombreArchivo || 'Reporte.pdf').replace(/[/\\?%*:|"<>]/g, '_');
        if (!cleanName.toLowerCase().endsWith('.pdf')) {
            cleanName += '.pdf';
        }

        // 1. Abrir en pestaña nueva con visor nativo
        if (relativeUrl) {
            window.open(relativeUrl, '_blank');
        }

        // 2. Disparar también la descarga automática directa
        const byteCharacters = atob(base64String);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: 'application/pdf' });
        const url = URL.createObjectURL(blob);
        const anchor = document.createElement('a');
        anchor.href = url;
        anchor.download = cleanName;
        document.body.appendChild(anchor);
        anchor.click();
        setTimeout(() => {
            anchor.remove();
            URL.revokeObjectURL(url);
        }, 30000);
    } catch (err) {
        console.error("Error al procesar descarga y apertura de PDF:", err);
    }
};