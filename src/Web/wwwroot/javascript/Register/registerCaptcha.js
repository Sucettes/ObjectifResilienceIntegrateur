function onSubmit(token) {
    document.getElementById("demo-test").submit();
}

$('[data-registerRequest]').on('click', event => {
    document.getElementById('msgErreurPassword').textContent = "";
    document.getElementById('msgErreurPasswordLength').textContent = "";
    document.getElementById('msgErreurConfPassword').textContent = "";
    document.getElementById('msgErreurCourriel').textContent = "";
    if (document.getElementById('password').value != "" && document.getElementById('confPassword').value != "" && document.getElementById('courriel').value != "") {
        $.ajax({
            type: 'POST',
            url: window.location.pathname,
            async: false,
            data: new FormData(document.getElementById('form-register')),
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.msgSuccess != null) {
                    $('.msgSucceeded').append("<div class='alert alert-primary' role='alert'>" + data.msgSuccess + "</div>");
                    setTimeout(function () {
                        window.location.href = '/Index';
                    }, 10000);
                }
                else {
                    if (data.erreurPassword != null) {
                        document.getElementById('msgErreurPassword').textContent = data.erreurPassword;
                        document.getElementById('msgErreurConfPassword').textContent = data.erreurPassword;
                    }
                    if (data.erreurCourriel != null) {
                        document.getElementById('msgErreurCourriel').textContent = data.erreurCourriel;
                    }
                }
            },
            error: function () {
                document.getElementById('msgErreurCourriel').textContent = "Ce courriel est déjà lié à un compte";
            }
        });
    }
    else {
        if (document.getElementById('password').value == "") {
            document.getElementById('msgErreurPassword').textContent = "Le champ ne peut pas être vide.";
        }
        if (document.getElementById('confPassword').value == "") {
            document.getElementById('msgErreurConfPassword').textContent = "Le champ ne peut pas être vide.";
        }
        if (document.getElementById('courriel').value == "") {
            document.getElementById('msgErreurCourriel').textContent = "Le champ ne peut pas être vide.";
        }
    }
});