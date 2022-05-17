'use strict';
$(document).ready(function () {
    // Création du tuto.
    $('[data-modificationTuto]').on('click', event => {
        event.preventDefault();
        let curTarget = event.currentTarget;
        var href = window.location.pathname + "?handler=ModifieTutorielDetails";
        console.log(curTarget.form)
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
                window.scriptToastNotification.AjouterNotification("Le tutoriel n'a pas été modifié!", false);
            }
        });
    });

    function modificationReussie(data) {
        $('#imgBanierre').attr('src', data.value.imgUrl);

        const state = { 'id': data.id, 'handler': 'CreeTutorielDetails' };
        const url = '/Tutoriel/CreationTuto';

        history.pushState(state, '', url);

        window.scriptToastNotification.AjouterNotification("Le tutoriel a été modifié!", true);
    }
});