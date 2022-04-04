(function () {
    'use strict';

    $(document).ready(function () {
        const params = new Proxy(new URLSearchParams(window.location.search), {
            get: (searchParams, prop) => searchParams.get(prop),
        });

        let value = params.modificationTutoStatus;

        var toastRD = document.getElementById('toastTutoModifie');

        // QUand le toast a fini de ce cacher
        toastRD.addEventListener('hide.bs.toast', function () {
            $('#toastTutoModifieDiv').attr('hidden');
        });

        // Quand le toast a fini de ce montrer
        toastRD.addEventListener('show.bs.toast', function () {
            $('#toastTutoModifieDiv').removeAttr('hidden');
        });

        if (value == 'true') {
            var toastObj = new bootstrap.Toast(toastRD)

            toastObj.show();

        }
    });
}());