'use strict';
$(document).ready(function () {
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
                window.scriptToastNotification.AjouterNotification("Le tutoriel n'a pas été créé!", false);
            }
        });
    });

    function creationReussie(data) {
        $('#imgBanierre').attr('src', data.value.imgUrl);
        const state = { 'id': data.value.idTutoP, 'handler': 'CreeTutorielDetails' };
        const url = '/Tutoriel/CreationTuto';

        history.pushState(state, '', url);

        $('[name="idTutoP"]').val(data.value.idTutoP);
        $('[id="idTutoP"]').val(data.value.idTutoP);
        $('[id="idTutoP2"]').val(data.value.idTutoP);
        $('[name="idtutoVal"]').val(data.value.idTutoP);
        
        $('#btnAddRangee').removeClass('disabled');
        $('#btnCreeTuto').attr('hidden', '');
        $('#btnModifierTuto').removeAttr('hidden', '');

        var o = new Option(data.value.titre + ' (Non Publié)');
        o.value = data.value.idTutoP;
        o.selected = true;
        $('#selectTutoModifier').append(o);
        // lancement du toast
        window.scriptToastNotification.AjouterNotification('Le tutoriel a été créé!', true);
    }
});