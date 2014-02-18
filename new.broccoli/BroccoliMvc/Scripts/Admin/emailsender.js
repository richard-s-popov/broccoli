$(document).ready(function() {
    
});

function deleteMail(id, element)
{
    if (confirm('Are you sure?')) {
        $.get('/BroccoliTest/Admin/DeleteMail?mailId=' + id, function (data) {
            if (data.result) {
                element.parent().parent().slideUp(200);
            }
        });
    } else {
        // Do nothing!
    }
}