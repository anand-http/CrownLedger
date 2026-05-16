function eraseCookie(e) { createCookie(e, "", -1) } function createCookie(e, t, n) { if (n) { var o = new Date; o.setTime(o.getTime() + 24 * n * 60 * 60 * 1e3); var i = "; expires=" + o.toGMTString() } else i = ""; document.cookie = e + "=" + t + i + "; path=/" } function readCookie(e) { for (var t = e + "=", n = document.cookie.split(";"), o = 0; o < n.length; o++) { for (var i = n[o]; " " == i.charAt(0);)i = i.substring(1, i.length); if (0 == i.indexOf(t)) return i.substring(t.length, i.length) } return null }
function loginSkipfunction() {
    $('.submit-main').hide();
    $('.submit-loader').css('display', 'inline-block');
    try {

        var validatemsg = requiredFieldsforLogin($('#txtUserName'), "User Name cannot left blank") &
            requiredFieldsforLogin($('#txtEntityCode'), "Entity Code cannot left blank") &
            requiredFieldsforLogin($('#txtPassword'), "Password cannot left blank");

        if (!validatemsg) {
            return false;
        }
        if (!$('.input-checkbox100').is(':checked')) {
            return false;
        }
        var datatopost = {
            UserName: $('#txtUserName').val(),
            Password: $('#txtPassword').val(),
            EntityCode: $('#txtEntityCode').val()
        };
        $.ajax({
            url: "../login/SkipAndLogin",
            type: "POST",
            dataType: "json",
            data: datatopost,
            error: function (response) {
                if (response.statusCode == 'LE001') {
                    var $Validate = [{ Code: response.StatusCode, Msg: 'Please enter valid User Name' }], $ctrls = [$('#txtUserName')];
                    swal({
                        title: '<h2>Error</h2>',
                        text: '<p>Please enter valid User Name</p>',
                        type: 'error',
                        html: true,
                        showCancelButton: false
                    });
                }
                else if (response.statusCode == 'LE002') {
                    var $Validate = [{ Code: response.StatusCode, Msg: 'Please enter valid Password' }], $ctrls = [$('#txtPassword')];
                    swal({
                        title: '<h2>Error</h2>',
                        text: '<p>Please enter valid Password</p>',
                        type: 'error',
                        html: true,
                        showCancelButton: false
                    });
                    // swalAlertWithValidation(response.StatusCode, $ctrls, $Validate);
                }
                else if (response.statusCode == 'LE003') {
                    var $Validate = [{ Code: response.StatusCode, Msg: 'User Name is disabled. Please check with Administrator' }], $ctrls = [$('#txtUserName')];
                    swal({
                        title: '<h2>Error</h2>',
                        text: '<p>User Name is disabled. Please check with Administrator</p>',
                        type: 'error',
                        html: true,
                        showCancelButton: false
                    });
                }
                else {
                    var $Validate = [{ Code: 'LE001', Msg: 'Entity Code is not valid. Please check with Administrator' }], $ctrls = [$('#txtEntityCode')];
                    swal({
                        title: '<h2>Error</h2>',
                        text: '<p>Entity Code is not valid. Please check with Administrator</p>',
                        type: 'error',
                        html: true,
                        showCancelButton: false
                    });
                }
                $('.submit-main').css('display', 'inline-block');
                $('.submit-loader').hide();
            },
            failure: function (response) {
                $('.submit-main').css('display', 'inline-block');
                $('.submit-loader').hide();
                swalValidationAlert('', 'error', response);
            },
            success: function (response) {
                var status = response;
                if (status == 'EN001') {
                    swal({
                        title: '<h2>Error</h2>',
                        text: '<p>Entity Code does not match</p>',
                        type: 'error',
                        html: true,
                        showCancelButton: false
                    });
                    $('.submit-main').css('display', 'inline-block');
                    $('.submit-loader').hide();
                    return;
                }
                if (status == 'EE001') {
                    swal({
                        title: "Your licence has been expire",
                        text: 'Do you want to continue',
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#f56c40",
                        confirmButtonText: "Yes",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $("#modalReniewLicence").modal('show');
                                $('#txtmEntityCode').val('');
                                $('#txtEntityEmail').val('');
                                $('#txtOTP').val('');
                                $('.dvvalidateemail').show();
                                $('.dvverifyCode').hide();
                                $('.dvlicencepaymentdetail').hide();
                            }
                        });
                    $('.submit-main').css('display', 'inline-block');
                    $('.submit-loader').hide();
                }
                else {
                    
                    if (status == 'Dashboard') {
                        if (readCookie("Pin") != null) {
                            if (readCookie("Pin") == 'PersonalDashboard') {
                                window.location.href = "../Dashboard/PersonalDashboard";
                            }
                            if (readCookie("Pin") == 'Dashboard') {
                                window.location.href = "../Dashboard/Dashboard";
                            }
                        }
                        else {
                            window.location.href = "../Dashboard/Dashboard";
                        }

                    }

                    else {
                        $('.submit-main').css('display', 'inline-block');
                        $('.submit-loader').hide();
                        var code = !!SUCCESSCODES[status] ? SUCCESSCODES[status].value : status;
                        $.ajax({ url: "../Login/LogOut", type: "POST", dataType: "json" });
                        swal({
                            title: '<h2>Error</h2>',
                            text: '<p>' + code + '</p>',
                            type: 'error',
                            html: true,
                            showCancelButton: false
                        });
                    }
                }
                $('.submit-main').css('display', 'inline-block');
                $('.submit-loader').hide();
            }
        });
    } catch (err) {
        swal({
            title: '<h2>Error</h2>',
            text: '<p>' + err.message + '</p><p>' + err.description + '</p>',
            type: 'error',
            html: true,
            showCancelButton: false
        });
        $('.submit-main').css('display', 'inline-block');
        $('.submit-loader').hide();
    }
}