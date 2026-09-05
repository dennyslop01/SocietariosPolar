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

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}