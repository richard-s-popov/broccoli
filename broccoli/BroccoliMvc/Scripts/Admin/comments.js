$(document).ready(function() {
    $('#adminPage').hide();
    

});

function ApproveComment(id) {
    $.ajax({
        url: document.ApproveCommentUrl,
        data: {
            id: id
        },
        success: function(data) {
            $('#comment-' + id).html('<span style="text-align: center;">Отзыв одобрен</span>');
        }
    });
}

function DeclineComment(id) {
    $.ajax({
        url: document.DeclineCommentUrl,
        data: {
            id: id
        },
        success: function (data) {
            $('#comment-' + id).html('<span style="text-align: center;">Отзыв отклонен</span>');
        }
    });
}