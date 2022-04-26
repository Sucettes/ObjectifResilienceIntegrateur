$(document).ready(function () {
    $('[data-filtreRecherche]').on('click', event => {
        event.preventDefault();
        event.stopPropagation();
        window.scriptRechercheUtils.rechercherTuto(event);
    });
    $('[data-paginationBtn]').on('click', event => {
        //event.preventDefault();
        //event.stopPropagation();
        window.scriptRechercheUtils.setPaginationClick;
    });


    window.addEventListener('beforeunload', () => delete window.scriptRechercheUtils);
    window.scriptRechercheUtils.rechercherTuto();
});