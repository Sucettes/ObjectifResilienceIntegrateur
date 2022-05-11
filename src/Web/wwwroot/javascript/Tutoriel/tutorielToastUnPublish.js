(function () {
    'use strict';

    $(document).ready(function () {
        const params = new Proxy(new URLSearchParams(window.location.search), {
            get: (searchParams, prop) => searchParams.get(prop),
        });

        if (params.unPublishStatus == 'true') {
            window.scriptToastNotification.AjouterNotification('Le tutoriel a été retiré du mode public!', true);
        } else if (params.unPublishStatus == 'false'){
            window.scriptToastNotification.AjouterNotification('Le tutoriel a été publié!', true);
        }

    });
}());
