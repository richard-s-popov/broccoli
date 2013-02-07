$(document).ready(function () {
    var inviteList = [];
    var src = $('#srcCross').attr('src');
    
    // Обработчик добавления полей для ввода нового пользователя
    $('#addUser').click(function () {
        $('#assignedUsers tr:last').after('<tr><td></td><td class="inputs"><input class="nameAssignedUser" type="text"></td><td class="inputs"><input class="emailAssignedUser" type="text"></td><td class="inputs"><div class="for-img"><img class="deleteAssignedUserBtn" src="' + src + '"/></div></td></tr>');

        redrawRows();
    });

    // Обработчик удаления полей
    $('.deleteAssignedUserBtn').live('click', function() {
        $(this).parent().parent().parent().remove();

        redrawRows();
    });
    
    // Обработчик для отображения контейнера приглашений
    $('#showAltWayContainer').click(function () {
        $('.alt-way-container').css('display', 'block');
        $('.alt-way-container').css('top', '0');
        $(this).hide();
        redrawRows();
    });

    $('#sendInvites').click(function() {
        prepareInviteList();
        sendInviteList();
    });

    $('#message').keydown(function() {
        var less = 2000 - $(this).val().length;
        
        if (less < 0) {
            $(this).val($(this).val().substring(0, 1999));
            less = 0;
        }

        $('#lessSymbols').html('Осталось ' + less + ' символов');
    });

    // Перекрашивание строк таблицы и перерасчет нумерации строк
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

    // Подготовка/наполнение массива данными пользователей
    function prepareInviteList() {
        $.each($('#assignedUsers tr:not(:first)'), function (index) {
            var name = $(this).find('.nameAssignedUser').val();
            var email = $(this).find('.emailAssignedUser').val();

            if (index == 0) {
                inviteList = [{ name: name, email: email }];
            } else {
                inviteList.push({ name: name, email: email });
            }
        });
    }

    // Отправка инвайт листа на сервер
    function sendInviteList() {
        $.ajax({
            type: "POST",
            url: document.SendInvitesList,
            data: {
                inviteList: JSON.stringify(inviteList),
                message: $('#message').val()
            },
            dataType: "json",
            traditional: true
        });
    }
});