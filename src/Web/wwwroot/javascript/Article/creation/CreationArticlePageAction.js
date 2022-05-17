'use strict';
$(document).ready(function () {
    // Création du article.
    let id;
    $('[data-creationTuto]').on('click', event => {

        creationReussie();

        function creationReussie() {
            // lancement du toast
            window.scriptToastNotification.AjouterNotification('Le tutoriel a été créé!', true);
        }
    }
});