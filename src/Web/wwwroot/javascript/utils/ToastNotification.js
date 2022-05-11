/*
 Comment l'utilisé:
 Importer le fichier dans le html : <script src="~/javascript/utils/ToastNotification.js" asp-append-version="true"></script>
 utilisé : window.scriptToastNotification.AjouterNotification('message', false ou true);
 */
(function () {
    'use strict';

    var toast;
    let scriptToastNotification = {
        /**
         * Permet de créé le toast pour les notification.
         * @param {any} msg Le contennue du toast (le message en string)
         * @param {any} valide true ou false, si c'est un sucess ou un erreur.
         */
        AjouterNotification: (msg, valide) => {
            scriptToastNotification.RetirerNotification();

            // Création des éléments dans des variable
            let $divToastParent = $('<div id="toastNotificationDiv" class="position-fixed bottom-0 end-0 p-3" style="z-index: 11" hidden></div>');
            let $divToastContent = $('<div id="toastNotification" class="toast" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="true" data-bs-animation="true" data-bs-delay="3500"></div>');

            let $divToastHead = $('<div class="toast-header"></div>');
            let $divToastBody = $('<div class="toast-body">' + msg + '</div>');

            let $imgToastIcon;
            if (valide === true) {
                $imgToastIcon = $('<img style="max-width:1.5rem;max-height:1.5rem;" src="/icons/valide.svg" class="rounded me-2" alt="...">');
            } else {
                $imgToastIcon = $('<img style="max-width:1.5rem;max-height:1.5rem;" src="/icons/error.svg" class="rounded me-2" alt="...">');
            }
            let $strongMsg = $('<strong class="me-auto">Information!</strong>');
            let $smallMsg = $('<small>Nouveau!</small>');
            let $btnClose = $('<button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>');

            // Fusion de touts les éléments ensemble
            $divToastHead.append($imgToastIcon);
            $divToastHead.append($strongMsg);
            $divToastHead.append($smallMsg);
            $divToastHead.append($btnClose);

            $divToastContent.append($divToastHead);
            $divToastContent.append($divToastBody);
            $divToastParent.append($divToastContent);

            $('body').append($divToastParent);

            // Ajouts du listener d'evenement.
            toast = document.getElementById('toastNotification');
            // Quand le toast a fini de ce cacher
            toast.addEventListener('hide.bs.toast', function () {
                $('#toastNotificationDiv').attr('hidden');
            });

            // Quand le toast a fini de ce montrer
            toast.addEventListener('show.bs.toast', function () {
                $('#toastNotificationDiv').removeAttr('hidden');
            });

            if ('true' === 'true') {
                var toastObj = new bootstrap.Toast(toast)
                toastObj.show();
            }
        },
        /** Permet de supprimer le toast qui a été créé. */
        RetirerNotification: () => {
            $('#toastNotificationDiv').remove();
        }
    }

    window.scriptToastNotification = scriptToastNotification;
}());

