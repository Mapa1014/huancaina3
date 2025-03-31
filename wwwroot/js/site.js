document.addEventListener('DOMContentLoaded', function () {
    console.log("DOM completamente cargado");
});


// Función para agregar eventos a los enlaces de navegación
function attachNavEvents() {
    document.querySelectorAll('a[data-controller]').forEach(link => {
        link.removeEventListener('click', handleNavClick); // Elimina eventos duplicados
        link.addEventListener('click', handleNavClick);
    });
}

// Maneja el clic en los enlaces
function handleNavClick(e) {
    e.preventDefault(); // Evita la recarga de la página

    let controller = this.getAttribute('data-controller');
    let action = this.getAttribute('data-action');
    if (!controller || !action) return;

    loadPage(controller, action);
}

// Cargar la página con fetch
function loadPage(controller, action) {
    let url = `/${controller}/${action}`;

    fetch(url, {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(response => {
            if (!response.ok) throw new Error('Error al cargar la vista');
            return response.text();
        })
        .then(data => {
            let mainContent = document.getElementById('main-content');
            mainContent.innerHTML = data;
            history.pushState(null, '', url);

            // **REACTIVAR Bootstrap y otros scripts**
            reinitializeScripts();
            attachNavEvents(); // Reasignar eventos de navegación a nuevos enlaces
        })
        .catch(error => console.error('Error:', error));
}

// Reactivar Bootstrap y otros scripts después de cargar contenido dinámico
function reinitializeScripts() {
    // Reactivar todos los carruseles de Bootstrap
    document.querySelectorAll('.carousel').forEach(carousel => {
        new bootstrap.Carousel(carousel);
    });
}


// Manejar navegación con los botones "atrás" y "adelante"
window.addEventListener('popstate', () => {
    const path = window.location.pathname.split('/').filter(Boolean);
    if (path.length < 2) return;
    loadPage(path[0], path[1]);
});


<script>
        // Validación adicional para los campos de teléfono
    document.addEventListener('DOMContentLoaded', function() {
            const telefonos = document.querySelectorAll('input[type="tel"]');

    telefonos.forEach(function(tel) {
        tel.addEventListener('input', function () {
            // Eliminar cualquier caracter que no sea número
            this.value = this.value.replace(/[^0-9]/g, '');

            // Validación visual
            const formGroup = this.closest('.col-md-12');
            if (this.value.length === 10 && /^[0-9]{10}$/.test(this.value)) {
                formGroup.classList.remove('is-invalid');
                formGroup.classList.add('is-valid');
            } else {
                formGroup.classList.remove('is-valid');
                formGroup.classList.add('is-invalid');
            }
        });
            });
        });
</script>


