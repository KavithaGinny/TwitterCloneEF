$(document).ready(function () {
    LoadData();
    LoadFollwingAndFollwerCount();
    var text_max = 130;
    $('#textarea_feedback').html(text_max + ' characters remaining');

    $('#MessageUpd').keyup(function () {
        var text_length = $('#MessageUpd').val().length;
        var text_remaining = text_max - text_length;

        $('#textarea_feedback').html(text_remaining + ' characters remaining');
    });

    $('#txtSearchPerson').keyup(function () {
        var SrhPersontxt = $("#txtSearchPerson").val();
        $.ajax({
            url: "/Home/SearchPerson",
            method: "POST",
            data: '{ "PersonUserId": "' + SrhPersontxt + '"}',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (datas) {
                datas = JSON.parse(datas);
                if (datas.PersonID != "") {
                    $('#shrMsg').empty();
                    $('#followdiv').show();
                    $("#spnSrhUser").text(datas.PersonID);

                    if (datas.FollowCheck == "0") {

                        $("#followdiv").find('#btnfollowUnfollow').addClass("text-success");
                        $("#followdiv").find('#btnfollowUnfollow').text("Follow").css('cursor', 'pointer');
                    }
                    else {
                        $("#followdiv").find('#btnfollowUnfollow').addClass("text-warning");
                        $("#followdiv").find('#btnfollowUnfollow').text("Unfollow").css('cursor', 'pointer');;
                    }
                }
                else {
                    $('#shrMsg').removeClass("dipNone");
                    $('#shrMsg').addClass("dipBlock alert-danger");
                    $("#shrMsg").text("search not found");
                }
            },
            error: function (ex) {
                var r = jQuery.parseJSON(response.responseText);
                alert("Message: " + r.Message);
                alert("StackTrace: " + r.StackTrace);
                alert("ExceptionType: " + r.ExceptionType);
            }
        });
        return false;
    });

    $("#btnfollowUnfollow").click(function () {

        var FollowPerson = $("#followdiv").find('#spnSrhUser').html();

        $.ajax({

            url: "/Home/FollowPerson",
            method: "POST",
            data: '{ "PersionUserId": "' + FollowPerson + '"}',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (Message) {
                Message = JSON.parse(Message);

                if (Message == "SUCCESS") {
                    $("#followdiv").find('#btnfollowUnfollow').addClass("text-warning");
                    $("#followdiv").find("#btnfollowUnfollow").text("Unfollow");
                }
                else {
                    $("#followdiv").find('#btnfollowUnfollow').addClass("text-success");
                    $("#followdiv").find("#btnfollowUnfollow").text("Follow");
                }
                LoadFollwingAndFollwerCount();
            },
            error: function (error) {

            }
        });
        return false;

    });



    $("#btnUpdateMsg").click(function () {
        if (validateMessage()) {
            var Messagetxt = $("#MessageUpd").val();
            $.ajax({

                url: "/Home/PostTwitter",
                method: "POST",
                data: '{ "messagePost": "' + Messagetxt + '"}',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    $("#MessageUpd").val('');
                    $("#spnSuccess").removeClass("alert-danger");
                    $("#spnSuccess").addClass("alert-success");
                    $("#spnSuccess").text(" Posted successfully.");
                    LoadData();
                },
                error: function (error) {
                    $("#spnSuccess").addClass("alert-danger");
                    $("#spnSuccess").text(" Error while posting");
                    console.log(error);
                }
            });
            return false;
        }
    });
});


function LoadData() {
    $('#tweetsDetails').empty();

    $.ajax({
        url: "/Home/GetTwitter",
        method: "GET",
        dataType: 'json',
        success: function (data) {
            data = JSON.parse(data);
            $.each(data, function (i, item) {
                var rows = "<h5>"
                    + "<span class='spanUserID form-control'>" + item.user_id + "    " + item.created + "</span>"
                    + "<label class='spanMessage '>" + item.messaage + "</label>"
                    + "</h5>";
                $('#tweetsDetails').append(rows);
            });
            LoadTweetCount();
        },
        error: function (ex) {
            var r = jQuery.parseJSON(response.responseText);
            alert("Message: " + r.Message);
            alert("StackTrace: " + r.StackTrace);
            alert("ExceptionType: " + r.ExceptionType);
        }
    });
    return false;
}

function LoadTweetCount() {

    $.ajax({
        url: "/Home/GetTwittecount",
        method: "GET",
        dataType: 'json',
        success: function (data) {
            data = JSON.parse(data);
            $("#twtCnt u").text(data);
        },
        error: function (ex) {
            var r = jQuery.parseJSON(response.responseText);
            alert("Message: " + r.Message);
            alert("StackTrace: " + r.StackTrace);
            alert("ExceptionType: " + r.ExceptionType);
        }
    });
    return false;
}

function LoadFollwingAndFollwerCount() {

    $.ajax({
        url: "/Home/GetFollowingFollowercount",
        method: "GET",
        dataType: 'json',
        success: function (data) {
            data = JSON.parse(data);
            $("#FollwingCnt u").text(data.FollowingCnt);
            $("#FollwertCnt u").text(data.FollowerCnt);
        },
        error: function (ex) {
            var r = jQuery.parseJSON(response.responseText);
            alert("Message: " + r.Message);
            alert("StackTrace: " + r.StackTrace);
            alert("ExceptionType: " + r.ExceptionType);
        }
    });
    return false;
}
function validateMessage() {
    if ($("#MessageUpd").val() == '') {
        $("#spnSuccess").removeClass("alert-success");
        $("#spnSuccess").addClass("alert-danger");
        $("#spnSuccess").text(" Please enter your message to post");
        return false;
    }
    else {
        var text_max = 130;
        $('#textarea_feedback').html(text_max + ' characters remaining');
        return true;
    }

}