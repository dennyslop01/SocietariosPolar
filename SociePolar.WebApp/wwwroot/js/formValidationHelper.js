/**
 * Helper de Accesibilidad y UX para formularios Blazor
 * Hace scroll automático exacto y enfoca el primer campo inválido o con mensaje de error.
 */

window.focusFirstInvalidField = function (modalOrFormSelector) {
    const executeFocus = () => {
        let container = null;
        if (modalOrFormSelector) {
            container = document.querySelector(modalOrFormSelector);
        }
        if (!container) {
            container = document.querySelector('.modal.show') || document.querySelector('.modal') || document;
        }

        // 1. Localizar el primer elemento con mensaje de validación o clase inválida
        let target = null;
        
        // Buscar primero mensajes de error visibles con texto
        const validationMessages = container.querySelectorAll('.validation-message');
        for (const msg of validationMessages) {
            if (msg.textContent && msg.textContent.trim().length > 0) {
                target = msg;
                break;
            }
        }

        // Si no hay validation-message, buscar selectores de inputs inválidos
        if (!target) {
            const candidateSelectors = [
                '.is-invalid',
                '.invalid',
                'input.modified.invalid',
                'select.modified.invalid',
                'textarea.modified.invalid',
                ':invalid:not(form)',
                '[aria-invalid="true"]'
            ];
            for (const sel of candidateSelectors) {
                const el = container.querySelector(sel);
                if (el) {
                    target = el;
                    break;
                }
            }
        }

        if (target) {
            // Obtener el control real a enfocar (input, select, textarea o botón de dropdown)
            let inputToFocus = target;
            if (target.classList.contains('validation-message')) {
                const parentGroup = target.closest('.mb-3, .form-group, .col-md-6, .col-md-4, .col-md-12, .col, .dropdown, div');
                if (parentGroup) {
                    inputToFocus = parentGroup.querySelector('input:not([type="hidden"]), select, textarea, button.form-select') || target;
                }
            }

            // 2. Función para encontrar el contenedor con scroll activo
            function getScrollableContainer(el) {
                let curr = el.parentElement;
                while (curr && curr !== document.body && curr !== document.documentElement) {
                    const style = window.getComputedStyle(curr);
                    const overflowY = style.overflowY;
                    if ((overflowY === 'auto' || overflowY === 'scroll' || overflowY === 'overlay') && curr.scrollHeight > curr.clientHeight) {
                        return curr;
                    }
                    if (curr.classList.contains('modal-body') || curr.classList.contains('modal')) {
                        return curr;
                    }
                    curr = curr.parentElement;
                }
                return document.querySelector('.modal.show .modal-body') || document.querySelector('.modal-body') || document.querySelector('.modal.show') || window;
            }

            const scrollParent = getScrollableContainer(inputToFocus);

            // Desplazar el contenedor con scroll de forma suave y centrada
            if (scrollParent && scrollParent !== window && typeof scrollParent.scrollTo === 'function') {
                const parentRect = scrollParent.getBoundingClientRect();
                const targetRect = inputToFocus.getBoundingClientRect();
                const relativeTop = targetRect.top - parentRect.top;
                
                scrollParent.scrollTo({
                    top: scrollParent.scrollTop + relativeTop - (scrollParent.clientHeight / 2) + (targetRect.height / 2),
                    behavior: 'smooth'
                });
            } else {
                inputToFocus.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }

            // Si hay un modal padre con scroll adicional, también desplazarlo
            const outerModal = inputToFocus.closest('.modal');
            if (outerModal && outerModal !== scrollParent && outerModal.scrollHeight > outerModal.clientHeight) {
                const mRect = outerModal.getBoundingClientRect();
                const tRect = inputToFocus.getBoundingClientRect();
                outerModal.scrollTo({
                    top: outerModal.scrollTop + (tRect.top - mRect.top) - (outerModal.clientHeight / 2),
                    behavior: 'smooth'
                });
            }

            // 3. Foco inmediato en el control
            if (typeof inputToFocus.focus === 'function') {
                try {
                    inputToFocus.focus({ preventScroll: true });
                } catch (e) {
                    inputToFocus.focus();
                }
            }

            // 4. Aplicar efecto visual de pulso y borde rojo
            inputToFocus.classList.remove('pulse-invalid-field');
            void inputToFocus.offsetWidth; // Forzar reflow
            inputToFocus.classList.add('pulse-invalid-field');
            
            setTimeout(() => {
                inputToFocus.classList.remove('pulse-invalid-field');
            }, 3000);

            return true;
        }
        return false;
    };

    // Ejecutar con reintentos para asegurar renderizado tras WebSocket
    if (!executeFocus()) {
        setTimeout(executeFocus, 50);
        setTimeout(executeFocus, 150);
        setTimeout(executeFocus, 350);
        setTimeout(executeFocus, 700);
    }
};

// Observador global de mutaciones para detectar mensajes de error que aparezcan en el DOM
const observer = new MutationObserver(function (mutations) {
    for (const mutation of mutations) {
        if (mutation.type === 'childList') {
            for (const node of mutation.addedNodes) {
                if (node.nodeType === 1) {
                    if (node.classList && node.classList.contains('validation-message') && node.textContent.trim().length > 0) {
                        window.focusFirstInvalidField();
                        return;
                    }
                    if (node.querySelector && node.querySelector('.validation-message')) {
                        const vm = node.querySelector('.validation-message');
                        if (vm && vm.textContent.trim().length > 0) {
                            window.focusFirstInvalidField();
                            return;
                        }
                    }
                }
            }
        }
    }
});

if (document.body) {
    observer.observe(document.body, { childList: true, subtree: true });
} else {
    document.addEventListener('DOMContentLoaded', function () {
        observer.observe(document.body, { childList: true, subtree: true });
    });
}

// Click listener en botones submit
document.addEventListener('click', function (e) {
    const submitBtn = e.target.closest('button[type="submit"], .btn-primary');
    if (!submitBtn) return;

    setTimeout(() => window.focusFirstInvalidField(), 80);
    setTimeout(() => window.focusFirstInvalidField(), 250);
    setTimeout(() => window.focusFirstInvalidField(), 500);
}, true);
