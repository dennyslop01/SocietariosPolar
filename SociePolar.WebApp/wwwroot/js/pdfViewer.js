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
    // 1. Asegurar la extensión correcta
    const nombreConExtension = nombreArchivo.endsWith('.xlsx') ? nombreArchivo : `${nombreArchivo}.xlsx`;

    try {
        // 2. Convertir el string Base64 a bytes binarios
        const caracteresBinarios = atob(base64String);
        const numerosBinarios = new Array(caracteresBinarios.length);

        for (let i = 0; i < caracteresBinarios.length; i++) {
            numerosBinarios[i] = caracteresBinarios.charCodeAt(i);
        }

        const matrizBytes = new Uint8Array(numerosBinarios);

        // 3. Crear el Blob con el tipo MIME de Excel (XLSX)
        const blobExcel = new Blob([matrizBytes], {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        });

        // 4. Crear una URL temporal para el objeto Blob
        const urlBlob = URL.createObjectURL(blobExcel);

        // 5. Abrir en una nueva pestaña (forzará la descarga/apertura en el cliente)
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

