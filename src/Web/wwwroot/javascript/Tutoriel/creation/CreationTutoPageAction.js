'use strict';
$(document).ready(function () {

    let id;
    var toastRD = document.getElementById('toastTutoCree');
    // QUand le toast a fini de ce cacher
    toastRD.addEventListener('hide.bs.toast', function () {
        $('#toastTutoCreeDiv').attr('hidden');
    });

    // Quand le toast a fini de ce montrer
    toastRD.addEventListener('show.bs.toast', function () {
        $('#toastTutoCreeDiv').removeAttr('hidden');
    });

    // Création du tuto.
    $('[data-creationTuto]').on('click', event => {
        event.preventDefault();
        let curTarget = event.currentTarget;

        var href = window.location.pathname + "?handler=CreeTutorielDetails";

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
                creationReussie(data, curTarget);
            },
            error: function () {
            }
        });
    });

    function creationReussie(data) {
        var toastObj = new bootstrap.Toast(toastRD);

        $('#imgBanierre').attr('src', data.value.imgUrl);
        id = data.id;
        const state = { 'id': data.id, 'handler': 'CreeTutorielDetails' };
        const url = '/Tutoriel/CreationTuto';

        history.pushState(state, '', url);

        $('#btnCreeTuto').removeClass('btn btn-outline-primary');
        $('#btnCreeTuto').addClass('btn disabled btn-primary');
        $('#btnModifierTuto').removeClass('btn disabled btn-primary');
        $('#btnModifierTuto').addClass('btn btn-outline-primary');

        $('#btnPublieTuto').removeClass('btn disabled btn-primary');
        $('#btnPublieTuto').addClass('btn btn-outline-primary');

        $('#idTutoP').val(data.value.idTutoP);

        toastObj.show();
    }
});