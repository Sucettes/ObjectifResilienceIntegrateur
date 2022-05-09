'use strict';
$(document).ready(function () {
    var toastRD = document.getElementById('toastTutoModifie');
    // QUand le toast a fini de ce cacher
    toastRD.addEventListener('hide.bs.toast', function () {
        $('#toastTutoModifieDiv').attr('hidden');
    });

    // Quand le toast a fini de ce montrer
    toastRD.addEventListener('show.bs.toast', function () {
        $('#toastTutoModifieDiv').removeAttr('hidden');
    });

    // Création du tuto.
    $('[data-modificationTuto]').on('click', event => {
        event.preventDefault();
        let curTarget = event.currentTarget;
        var href = window.location.pathname + "?handler=ModifieTutorielDetails";

        $.ajax({
            type: 'POST',
            url: href,
            data: new FormData(curTarget.form),
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                modificationReussie(data, curTarget);
            },
            error: function () {
                // 
            }
        });
    });

    function modificationReussie(data) {
        var toastObj = new bootstrap.Toast(toastRD);

        $('#imgBanierre').attr('src', data.value.imgUrl);

        const state = { 'id': data.id, 'handler': 'CreeTutorielDetails' };
        const url = '/Tutoriel/CreationTuto';

        history.pushState(state, '', url);

        toastObj.show();
    }
});