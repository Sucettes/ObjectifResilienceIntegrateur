$(document).ready(function () {
    $('[data-filtreRecherche]').on('click', event => {
        event.preventDefault();
        event.stopPropagation();
        window.scriptRechercheUtils.rechercherTuto(event);
    });
    //window.scriptRechercheUtils.rechercherTuto();

    window.addEventListener('beforeunload', () => delete window.scriptRechercheUtils);
});