$(document).ready(function () {
    var src = $('#srcCross').attr('src');
    
    $('#addUser').click(function () {
        $('#assignedUsers tr:last').after('<tr><td></td><td class="inputs"><input class="nameAssignedUser" type="text"></td><td class="inputs"><input class="emailAssignedUser" type="text"></td><td class="inputs"><div class="for-img"><img class="deleteAssignedUserBtn" src="' + src + '"/></div></td></tr>');

        redrawRows();
    });

    $('.deleteAssignedUserBtn').live('click', function() {
        $(this).parent().parent().remove();

        redrawRows();
    });
    
    $('#showAltWayContainer').click(function() {
        $('.alt-way-container').css('display', 'block');
        $('.alt-way-container').css('top', '0')
        $(this).hide();
        redrawRows();
    });

    function redrawRows() {
        var i = 1;
        $.each($('#assignedUsers td:first-child'), function () {
            $(this).html(i++);
            if (i % 2 == 0) {
                $(this).parent().addClass('bad');
            } else {
                $(this).parent().removeClass('bad');
            }
        });
    }
});