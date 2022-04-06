'use strict';

var curTarget;

$('[data-supprimer]').on('click', event => {
    event.preventDefault();
    event.stopPropagation();
    curTarget = event.currentTarget;
});

$('#btnConfirmDelete').click(function () {
    event.preventDefault();
    var href = window.location.pathname + "?handler=DeleteRange";
    $.ajax({
        type: 'POST',
        url: href,
        data: new FormData(curTarget.form),
        cache: false,
        contentType: false,
        processData: false,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            //window.location.replace(window.location + "&deleteRowResult=true");
            curTarget.form.parentElement.removeChild(curTarget.form)
        },
        error: function () {
            //window.location.replace(window.location + "&deleteRowResult=false");
        }
    });
});

