'use strict';
$(document).ready(function () {
    let idTuto;
    $('[data-supprimerTuto]').on('click', event => {
        event.preventDefault();
        event.stopPropagation();
        idTuto = event.target.parentElement[0].value;
    });

    $('#btnConfirmDeleteTuto').click(function () {
        var href = window.location.pathname + "?handler=DeleteTuto";

        var tutoVal = { 'tutorielIdVal': idTuto }

        $.ajax({
            type: 'POST',
            url: href,
            data: JSON.stringify(tutoVal),
            contentType: "application/json",
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            }
        });
    });

});