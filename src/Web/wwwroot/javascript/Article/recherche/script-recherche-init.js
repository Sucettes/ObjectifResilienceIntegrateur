(function () {
    'use strict';

    window.addEventListener('load', () => {
        $('[data-filtreRecherche]').on('click', event => {
            event.preventDefault();
            event.stopPropagation();
            window.scriptRechercheUtils.rechercherArticle(event);
        });
        $('[data-paginationBtn]').on('click', event => {
            window.scriptRechercheUtils.setPaginationClick;
        });

        window.scriptRechercheUtils.rechercherArticle();
    });
    window.addEventListener('beforeunload', () => delete window.scriptRechercheUtils);

})();
