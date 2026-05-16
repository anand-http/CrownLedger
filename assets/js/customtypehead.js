var SERVICEPATH = window.location.protocol + "//" + window.location.host + "/AppService/";

function RemoveAutoSpecialChar(e) {
    var t = $(this);
    setTimeout(function () {
        var e = t.val(),
            a = e.replace(/[^a-zA-Z0-9\b :.,_-]/g, "");
        e != a && t.val(a)
    })
}

function MultiselectAddons(e) {
    $(e).find("ul#ULSelectedAddons").length <= 0 && $(e).append('<ul style="display:none;" id="ULSelectedAddons"></ul><button type="button" class="btn btn-info btn-icon" id="btnSelectedItem"><i class="fa fa-arrows-alt"></i></button>'), $(e).find('button[id="btnSelectedItem"]').click(function () {
        $(window).width() > 767 ? $(this).siblings("ul#ULSelectedAddons").toggle("slow") : $(this).siblings("ul#ULSelectedAddons").css({
            top: $(window).width() / 2 - $(this).offset().top,
            left: $(window).width() / 2 - 112
        }).toggle("slow"), $(this).find("i").toggleClass("fa-arrows-alt fa-compress")
    }), $(e).find("input").focus(function () {
        $("ul#ULSelectedAddons").is(":visible") && ($("ul#ULSelectedAddons").hide("slow"), $(this).closest("td").find('button[id="btnSelectedItem"] i').addClass("fa-arrows-alt"), $(this).closest("td").find('button[id="btnSelectedItem"] i').removeClass("fa-compress")), $(e).find("input").val("")
    })
}

function MultiselectAddonsforBooking(e) {
    $(e).find("ul#ULSelectedAddons").length <= 0 && $(e).append('<ul style="display:none;" id="ULSelectedAddons"></ul><button type="button" class="btn btn-info btn-icon" id="btnSelectedItem"><i class="fa fa-arrows-alt"></i></button>'), $(e).find('button[id="btnSelectedItem"]').click(function () {
        $(window).width() > 767 ? $(this).siblings("ul#ULSelectedAddons").toggle("slow") : $(this).siblings("ul#ULSelectedAddons").css({
            top: $(window).width() / 2 - $(this).offset().top,
            left: $(window).width() / 2 - 112
        }).toggle("slow"), $(this).find("i").toggleClass("fa-arrows-alt fa-compress")
    }), $(e).find("input").focus(function () {
        $("ul#ULSelectedAddons").is(":visible") && ($("ul#ULSelectedAddons").hide("slow"), $(this).closest("td").find('button[id="btnSelectedItem"] i').addClass("fa-arrows-alt"), $(this).closest("td").find('button[id="btnSelectedItem"] i').removeClass("fa-compress"))
    })
}

function CheckVailidDate(e, t, a) {
    var o, n = !0;
    if ("" != $(e).val()) {
        try {
            o = $.datepicker.parseDate(t, $(e).val())
        } catch (e) {
            n = !1
        }
        if (0 == n) try {
            n = !0, o = $.datepicker.parseDate(a, $(e).val())
        } catch (e) {
            n = !1
        }
        1 == n ? $(e).datepicker({
            dateFormat: t
        }).datepicker("setDate", o) : (swalValidationAlert("Invalid Date", "error", "Please enter valid date."), $(e).val(""))
    }
}

function ManualEntryDateRestriction(e, t) {
    var a, o, n = DateDisplay,
        i = DateEntry,
        r = !0,
        l = new Date;
    if ("" != $(e).val()) {
        try {
            a = $.datepicker.parseDate(n, $(e).val())
        } catch (e) {
            r = !1
        }
        if (0 == r) try {
            r = !0, a = $.datepicker.parseDate(i, $(e).val())
        } catch (e) {
            r = !1
        }
        if (1 == r) {
            o = $.datepicker.formatDate(n, a), l = new Date;
            var s = a,
                d = (new Date, $.datepicker.formatDate(n, new Date(l.setTime(l.getTime() + 864e5 * (parseFloat(t) - 1)))), new Date);
            d.setHours(0, 0, 0, 0), s >= d ? o = $.datepicker.formatDate(n, a) : (swalValidationAlert("Invalid Date", "error", "Please enter valid date."), l = new Date, o = $.datepicker.formatDate(n, new Date(l.setTime(l.getTime() + 864e5 * (parseFloat(t) - 1)))))
        } else l = new Date, l = new Date(l.setTime(l.getTime() + 864e5 * (parseFloat(t) - 1))), swalValidationAlert("Invalid Date", "error", "Please enter valid date."), o = $.datepicker.formatDate(n, l);
        $(e).val(o)
    }
}

function ManualPssportExpiryRestriction(e, t) {
    var a, o, n = DateDisplay,
        i = DateEntry,
        r = !0,
        l = new Date;
    if ("" != $(e).val()) {
        try {
            a = $.datepicker.parseDate(n, $(e).val())
        } catch (e) {
            r = !1
        }
        if (0 == r) try {
            r = !0, a = $.datepicker.parseDate(i, $(e).val())
        } catch (e) {
            r = !1
        }
        if (1 == r) {
            o = $.datepicker.formatDate(n, a), l = new Date;
            var s = a,
                d = new Date;
            d.setHours(0, 0, 0, 0);
            var c = $.datepicker.formatDate(n, new Date(l.setTime(l.getTime() + 864e5 * (parseFloat(t) - 1))));
            if (new RegExp("^([0]?[1-9]|[1-2]\\d|3[0-1])-(JAN|FEB|MAR|APR|MAY|JUN|JULY|AUG|SEP|OCT|NOV|DEC)-[1-2]\\d{3}$", "i").test(c)) var p = new Date(c.replace(/-/g, " "));
            else {
                var u = c.split("-");
                p = new Date(u[2], u[1] - 1, u[0])
            }
            s >= d && s < p ? swal(PassportExpire, function (a) {
                a || (l = new Date, o = $.datepicker.formatDate(n, new Date(l.setTime(l.getTime() + 864e5 * (parseFloat(t) - 1)))), $(e).val(o), $(e).attr("title", o))
            }) : s >= p ? o = $.datepicker.formatDate(n, s) : (swalValidationAlert("Invalid Date", "error", "Please enter valid date."), l = new Date, o = $.datepicker.formatDate(n, new Date(l.setTime(l.getTime() + 864e5 * (parseFloat(t) - 1)))))
        } else l = new Date, l = new Date(l.setTime(l.getTime() + 864e5 * (parseFloat(t) - 1))), swalValidationAlert("Invalid Date", "error", "Please enter valid date."), o = $.datepicker.formatDate(n, l);
        $(e).val(o)
    }
}

function ManualEntryDate(e) {
    var t, a, o = DateDisplay,
        n = DateEntry,
        i = !0;
    if ("" != $(e).val()) {
        try {
            t = $.datepicker.parseDate(o, $(e).val())
        } catch (e) {
            i = !1
        }
        if (0 == i) try {
            i = !0, t = $.datepicker.parseDate(n, $(e).val())
        } catch (e) {
            i = !1
        }
        1 == i ? a = $.datepicker.formatDate(o, t) : (swalValidationAlert("Invalid Date", "error", "Please enter valid date."), a = $.datepicker.formatDate(o, new Date)), $(e).val(a)
    }
}

function ForamtdatetoServer(e) {
    var t = DateDisplay;
    return DateCheck = $.datepicker.parseDate(t, $(e).val()), $.datepicker.formatDate(DateServer, DateCheck)
}

function ForamtdatetoServerValue(e) {
    var t = DateDisplay;
    return DateCheck = $.datepicker.parseDate(t, e), $.datepicker.formatDate(DateServer, DateCheck)
}

function DisplayDateFormat(e) {
    var t = DateDisplay;
    return $.datepicker.formatDate(t, e)
}

function Timecheck(e) {
    /([01][0-9]|[02][0-3]):[0-5][0-9]/.test($(e).val()) ? ($(e).removeClass("error"), $(e).attr("data-toggle", ""), $(e).attr("title", "")) : ($(e).addClass("error"), $(e).attr("data-toggle", "tooltip"), $(e).attr("title", "Invalid time."), $(e).val("00:00"))
}

function returnReportColor(e, t) {
    return "lgtcolor" == e && null != t ? t : "lgtcolor" == e && (null == t || t.length < 1) ? "#0ebcf2" : "azure" == e ? "#3d4247" : "blue" == e ? "#0ebcf2" : "brown" == e ? "#B66672" : "green" == e ? "#3c948b" : "horizon" == e ? "#cf315a" : "lgt_blues" == e ? "#4F96B6" : "orange" == e ? "#ED8323" : "purple" == e ? "#1e2f75" : "red" == e ? "#e45e66" : void 0
}
jQuery.fn.extend({
    dynamicParamAutoComplete: function (e, t, a, o, n, i, r, l, s) {
        $(this).attr("type", "Search");
        var d = $(this),
            c = [],
            p = !0;
        return d.attr("title", d.val()), d.keypress(function (e) {
            if (13 == (e.keyCode || e.which) || 34 == (e.keyCode || e.which) || 39 == (e.keyCode || e.which)) return e.preventDefault(), !1
        }), d.typeahead({
            hint: !1,
            highlight: !1,
            minLength: 0,
            maxItem: 20,
            order: "asc"
        }, {
                name: "ID",
                displayKey: "DisplayText",
                source: function (o, s) {
                    var u, h = {},
                        f = [];
                    r.val() == l ? (h.Table = t, f = i) : (h.Table = e, f = n), h.Prefix = a, h.SearchText = d.val();
                    var m = "";
                    if (f && null != f)
                        for (var y = 0; y < f.length; y++) {
                            var v = $("#" + f[y].CtrName).val();
                            v.length > 0 && (v.indexOf(",") > 0 ? m += " and " + f[y].ParamName + " in('" + v.replace(/,/g, "','") + "')" : (m += " and " + f[y].ParamName, m += (f[y].opr && 1 == f[y].opr ? "!" : "") + "=" + ($.isNumeric(v) ? Number(v) : "'" + v + "'")))
                        }
                    m.length > 0 ? (u = SERVICEPATH + "ws_GetSuggestedValueWithCondition", h.WhereCond = m) : u = SERVICEPATH + "ws_GetSuggestedValue", $.ajax({
                        url: u,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(h),
                        success: function (e) {
                            (c = [], $.each(e, function (e, t) {
                                c.push({
                                    ID: t.ID,
                                    Code: t.Code,
                                    DisplayText: t.Name
                                })
                            }), $(window).width() > 767)
                                ? (d.closest("span.twitter-typeahead").find(".tt-dropdown-menu").removeClass("btm_top"), d.closest("div.mCSB_container").length && (d.closest("div.mCSB_container").css("overflow", "hidden"), Number(d.closest("tbody").height()) > Number(d.closest("div.mCSB_container").height())
                                    ? d.closest("div.mCSB_container").css("overflow", "visible") : d.closest("div.mCSB_container").css("overflow", "hidden")), d.closest("tbody").length > 0
                                        ? d.offset().top - d.closest("tbody").offset().top > 200 && d.offset().top + d.outerHeight() > $(window).innerHeight() / 2 && d.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top")
                                        : d.offset().top + d.outerHeight() > $(window).innerHeight() / 2 && d.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top"))
                                : d.offset().top + d.outerHeight() > $(window).innerHeight() / 2 && d.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                            p = 0 != c.length, s(c)
                        },
                        error: function (e) {
                            swalValidationAlert(e.responseText, "error", "")
                        },
                        failure: function (e) {
                            swalValidationAlert("", "error", e.responseText)
                        }
                    })
                },
                templates: {
                    empty: '<span class="TH_NoRecordFound">No Record Found<span>'
                }
            }).on("typeahead:selected", function (e, t) {
                if (o.val(t.ID), d.removeClass("error"), d.attr("title", t.DisplayText), d.removeAttr("data-original-title"), s && "function" == typeof s) {
                    var a = d.attr("d-typehead");
                    a && null != a ? (s(t, a, d), d.attr("data-original-title", "")) : s(t)
                }
            }).on("typeahead:autocompleted", function (e, t) {
                if (o.val(t.ID), d.attr("title", t.DisplayText), s && "function" == typeof s) {
                    var a = d.attr("d-typehead");
                    a && null != a ? s(t, a) : s(t)
                }
            }).on("typeahead:closed", function (e, t) {
                for (var a = !1, n = 0; n < c.length && !a; n++) c[n].DisplayText === d.val() && (a = !0);
                if (0 == p || "" == d.val() || c.length > 0 && 0 == a) return d.typeahead("val", ""), d.prop("title", ""), o.val("0"), !1
            }).bind("keypress", function (e) {
                var t = new RegExp("^[a-zA-Z0-9\b? :,_-]+$"),
                    a = String.fromCharCode(e.charCode ? e.charCode : e.which);
                return !(9 != e.keyCode && !e.ctrlKey && 86 != e.keyCode && 88 != e.keyCode && 46 != e.keyCode && 37 != e.keyCode && 39 != e.keyCode) || (t.test(a) ? void 0 : (e.preventDefault(), !1))
            }).bind("paste input", RemoveAutoSpecialChar), d
    }
}), jQuery.fn.extend({
    typeheadAutoComplete: function (e, t, a, o, n) {
        $(this).attr("type", "Search");
        var i = $(this),
            r = [],
            l = !0;
        return i.attr("title", i.val()), i.keypress(function (e) {
            if (13 == (e.keyCode || e.which) || 34 == (e.keyCode || e.which) || 39 == (e.keyCode || e.which)) return e.preventDefault(), !1
        }), i.typeahead({
            hint: !1,
            highlight: !1,
            minLength: 0,
            maxItem: 20,
            order: "asc"
        }, {
                name: "ID",
                displayKey: "DisplayText",
                source: function (a, n) {
                    var s, d = {};
                    d.Table = e, d.Prefix = t, d.SearchText = i.val();
                    var c = "";
                    if (o && null != o)
                        for (var p = 0; p < o.length; p++) {
                            var u = $("#" + o[p].CtrName).val();
                            o[p].OrCondi != "1" && null == u && (u = "0"), u != "0" &&  u.length > 0 && (c += " and " + o[p].ParamName, void 0 !== o[p].opr ? 1 == o[p].opr ? c += "!=" + ($.isNumeric(u) ? Number(u) : "'" + u + "'") : c += ">" + ($.isNumeric(u) ? Number(u) : "'" + u + "'") : c += "=" + ($.isNumeric(u) ? Number(u) : "'" + u + "'")) // Change on 04082023 State field  add on partner and clinet contacts
                            
                        }
                    c.length > 0 ? (s = SERVICEPATH + "ws_GetSuggestedValueWithCondition", d.WhereCond = c) : s = SERVICEPATH + "ws_GetSuggestedValue", $.ajax({
                        url: s,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(d),
                        success: function (e) {
                            if (r = [], $.each(e, function (e, t) {
                                r.push({
                                    ID: t.ID,
                                    Code: t.Code,
                                    DisplayText: t.Name,
                                    Product_Desc: t.Product_Desc,
                                    Product_HSNCode: t.Product_HSNCode,
                                    Selling_Price: t.Selling_Price,
                                    Cost_Price: t.Cost_Price,

                                })
                            }), $(window).width() > 767)
                                if (i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").removeClass("btm_top"), i.closest("table.smallTabTwitter").length > 0) {
                                    var t = i.offset().left - i.closest("table.smallTabTwitter").offset().left,
                                        a = i.offset().top - i.closest("table.smallTabTwitter").offset().top + i.height();
                                    i.closest("span.twitter-typeahead").css("position", "static"), i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").css("top", a).css("left", t).css("width", i.width())
                                } else {
                                    if (i.closest("div.mCSB_container").length && (i.closest("div.mCSB_container").css("overflow", "hidden"), Number(i.closest("tbody").height()) > Number(i.closest("div.mCSB_container").height()) ? i.closest("div.mCSB_container").css("overflow", "visible") : i.closest("div.mCSB_container").css("overflow", "hidden")), i.closest("tbody").length > 0) i.offset().top - i.closest("tbody").offset().top > 200 && i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                                    else i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top")
                                } else i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                            l = 0 != r.length, n(r)
                        },
                        error: function (e) {
                            swalValidationAlert(e.responseText, "error")
                        },
                        failure: function (e) {
                            swalValidationAlert(e.responseText, "error")
                        }
                    })
                },
                templates: {
                    empty: '<span class="TH_NoRecordFound">No Record Found<span>'
                }
            }).on("typeahead:selected", function (e, t) {
                var disname = t.DisplayText.split('-');
                var fname = "";
                if (disname.length > 1) {
                    for (r = 0; r <= disname.length - 1; r++) {
                        if (r > 0) {
                            fname = fname + disname[r] + ' ';
                        }
                    }
                }
                else {
                    fname = disname;
                }
                t.DisplayText = $.trim(fname);
                if (a.val(t.ID), i.removeClass("error"), i.prop("title", t.DisplayText), i.removeAttr("data-original-title"), n && "function" == typeof n) {
                    var o = i.attr("d-typehead");
                    o && null != o ? (n(t, o, i), i.attr("data-original-title", "")) : n(t)
                }
            }).on("typeahead:autocompleted", function (e, t) {
                if (a.val(t.ID), i.prop("title", t.DisplayText), n && "function" == typeof n) {
                    var o = i.attr("d-typehead");
                    o && null != o ? n(t, o) : n(t)
                }
            }).on("typeahead:closed", function (e, t) {
                for (var o = !1, n = 0; n < r.length && !o; n++) r[n].ID === a.val() && (o = !0);
                if (0 == l || "" == i.val() || r.length > 0 && 0 == o) return i.typeahead("val", ""), i.prop("title", ""), a.val("0"), !1
            }).bind("keypress", function (e) {
                var t = new RegExp("^[a-zA-Z0-9\b :.,_-]+$"),
                    a = String.fromCharCode(e.charCode ? e.charCode : e.which);
                return !(9 != e.keyCode && !e.ctrlKey && 86 != e.keyCode && 88 != e.keyCode && 46 != e.keyCode && 37 != e.keyCode && 39 != e.keyCode) || (t.test(a) ? void 0 : (e.preventDefault(), !1))
            }).bind("paste input", RemoveAutoSpecialChar), i
    }
 }),jQuery.fn.extend({
        typeheadAutoCompleteCustom: function (e, t, a, o, n) {
            $(this).attr("type", "Search");
            var i = $(this),
                r = [],
                l = !0;
            return i.attr("title", i.val()), i.keypress(function (e) {
                if (13 == (e.keyCode || e.which) || 34 == (e.keyCode || e.which) || 39 == (e.keyCode || e.which)) return e.preventDefault(), !1
            }), i.typeahead({
                hint: !1,
                highlight: !1,
                minLength: 0,
                maxItem: 20,
                order: "asc"
            }, {
                    name: "ID",
                    displayKey: "DisplayText",
                    source: function (a, n) {
                        var s, d = {};
                        d.Table = e, d.Prefix = t, d.SearchText = i.val();
                        var c = "";
                        if (o && null != o)
                            for (var p = 0; p < o.length; p++) {
                                var u = $("#" + o[p].CtrName).val();
                                null == u && (u = "0"), u.length > 0 && (c += " and " + o[p].ParamName, void 0 !== o[p].opr ? 1 == o[p].opr ? c += "!=" + ($.isNumeric(u) ? Number(u) : "'" + u + "'") : c += ">" + ($.isNumeric(u) ? Number(u) : "'" + u + "'") : c += "=" + ($.isNumeric(u) ? Number(u) : "'" + u + "'"))
                            }
                        c.length > 0 ? (s = SERVICEPATH + "ws_GetSuggestedValueWithCondition", d.WhereCond = c) : s = SERVICEPATH + "ws_GetSuggestedValue", $.ajax({
                            url: s,
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(d),
                            success: function (e) {
                                if (r = [], $.each(e, function (e, t) {
                                    r.push({
                                        ID: t.ID,
                                        Code: t.Code,
                                        DisplayText: t.Name
                                    })
                                }), $(window).width() > 767)
                                    if (i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").removeClass("btm_top"), i.closest("table.smallTabTwitter").length > 0) {
                                        var t = i.offset().left - i.closest("table.smallTabTwitter").offset().left,
                                            a = i.offset().top - i.closest("table.smallTabTwitter").offset().top + i.height();
                                        i.closest("span.twitter-typeahead").css("position", "static"), i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").css("top", a).css("left", t).css("width", i.width())
                                    } else {
                                        if (i.closest("div.mCSB_container").length && (i.closest("div.mCSB_container").css("overflow", "hidden"), Number(i.closest("tbody").height()) > Number(i.closest("div.mCSB_container").height()) ? i.closest("div.mCSB_container").css("overflow", "visible") : i.closest("div.mCSB_container").css("overflow", "hidden")), i.closest("tbody").length > 0) i.offset().top - i.closest("tbody").offset().top > 200 && i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                                        else i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top")
                                    } else i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                                l = 0 != r.length, n(r)
                            },
                            error: function (e) {
                                swalValidationAlert(e.responseText, "error")
                            },
                            failure: function (e) {
                                swalValidationAlert(e.responseText, "error")
                            }
                        })
                    },
                    templates: {
                        empty: '<a onclick="AddNew_Company();" class="TH_NoRecordFound">Add New Company<a>'
                    }
                }).on("typeahead:selected", function (e, t) {
                    var disname = t.DisplayText.split('-');
                    var fname = "";
                    if (disname.length > 1) {
                        for (r = 0; r <= disname.length - 1; r++) {
                            if (r > 0) {
                                fname = fname + disname[r] + ' ';
                            }
                        }
                    }
                    else {
                        fname = disname;
                    }
                    t.DisplayText = $.trim(fname);
                    if (a.val(t.ID), i.removeClass("error"), i.prop("title", t.DisplayText), i.removeAttr("data-original-title"), n && "function" == typeof n) {
                        var o = i.attr("d-typehead");
                        o && null != o ? (n(t, o, i), i.attr("data-original-title", "")) : n(t)
                    }
                }).on("typeahead:autocompleted", function (e, t) {
                    if (a.val(t.ID), i.prop("title", t.DisplayText), n && "function" == typeof n) {
                        var o = i.attr("d-typehead");
                        o && null != o ? n(t, o) : n(t)
                    }
                }).on("typeahead:closed", function (e, t) {
                    for (var o = !1, n = 0; n < r.length && !o; n++) r[n].ID === a.val() && (o = !0);
                    if (0 == l || "" == i.val() || r.length > 0 && 0 == o) return i.typeahead("val", ""), i.prop("title", ""), a.val("0"), !1
                }).bind("keypress", function (e) {
                    var t = new RegExp("^[a-zA-Z0-9\b :.,_-]+$"),
                        a = String.fromCharCode(e.charCode ? e.charCode : e.which);
                    return !(9 != e.keyCode && !e.ctrlKey && 86 != e.keyCode && 88 != e.keyCode && 46 != e.keyCode && 37 != e.keyCode && 39 != e.keyCode) || (t.test(a) ? void 0 : (e.preventDefault(), !1))
                }).bind("paste input", RemoveAutoSpecialChar), i
        }
}), jQuery.fn.extend({
    typeheadServiceforRateRule: function (e, t, a, o, n) {
        $(this).attr("type", "Search");
        var i = $(this),
            r = [],
            l = !0;
        return i.attr("title", i.val()), i.keypress(function (e) {
            if (13 == (e.keyCode || e.which) || 34 == (e.keyCode || e.which) || 39 == (e.keyCode || e.which)) return e.preventDefault(), !1
        }), i.typeahead({
            hint: !1,
            highlight: !1,
            minLength: 0,
            maxItem: 20,
            order: "asc"
        }, {
                name: "ID",
                displayKey: "DisplayText",
                source: function (a, n) {
                    var s, d = {};
                    d.Table = e, d.Prefix = t, d.SearchText = i.val();
                    var c = "";
                    if (o && null != o)
                        for (var p = 0; p < o.length; p++) {
                            var u = $("#" + o[p].CtrName).val();
                            null == u && (u = "0"), u.length > 0 && "0" != u && (c += " and " + o[p].ParamName, void 0 !== o[p].opr ? 1 == o[p].opr ? c += "!=" + ($.isNumeric(u) ? Number(u) : "'" + u + "'") : c += ">" + ($.isNumeric(u) ? Number(u) : "'" + u + "'") : c += "=" + ($.isNumeric(u) ? Number(u) : "'" + u + "'"))
                        }
                    c.length > 0 ? (s = SERVICEPATH + "ws_GetSuggestedValueWithCondition", d.WhereCond = c) : s = SERVICEPATH + "ws_GetSuggestedValue", $.ajax({
                        url: s,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(d),
                        success: function (e) {
                            if (r = [], $.each(e, function (e, t) {
                                r.push({
                                    ID: t.ID,
                                    Code: t.Code,
                                    DisplayText: t.Name
                                })
                            }), $(window).width() > 767)
                                if (i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").removeClass("btm_top"), i.closest("table.smallTabTwitter").length > 0) {
                                    var t = i.offset().left - i.closest("table.smallTabTwitter").offset().left,
                                        a = i.offset().top - i.closest("table.smallTabTwitter").offset().top + i.height();
                                    i.closest("span.twitter-typeahead").css("position", "static"), i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").css("top", a).css("left", t).css("width", i.width())
                                } else {
                                    if (i.closest("div.mCSB_container").length && (i.closest("div.mCSB_container").css("overflow", "hidden"), Number(i.closest("tbody").height()) > Number(i.closest("div.mCSB_container").height()) ? i.closest("div.mCSB_container").css("overflow", "visible") : i.closest("div.mCSB_container").css("overflow", "hidden")), i.closest("tbody").length > 0) i.offset().top - i.closest("tbody").offset().top > 200 && i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                                    else i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top")
                                } else i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                            l = 0 != r.length, n(r)
                        },
                        error: function (e) {
                            swalValidationAlert(e.responseText, "error")
                        },
                        failure: function (e) {
                            swalValidationAlert(e.responseText, "error")
                        }
                    })
                },
                templates: {
                    empty: '<span class="TH_NoRecordFound">No Record Found<span>'
                }
            }).on("typeahead:selected", function (e, t) {
                if (a.val(t.ID), i.removeClass("error"), i.prop("title", t.DisplayText), i.removeAttr("data-original-title"), n && "function" == typeof n) {
                    var o = i.attr("d-typehead");
                    o && null != o ? (n(t, o, i), i.attr("data-original-title", "")) : n(t)
                }
            }).on("typeahead:autocompleted", function (e, t) {
                if (a.val(t.ID), i.prop("title", t.DisplayText), n && "function" == typeof n) {
                    var o = i.attr("d-typehead");
                    o && null != o ? n(t, o) : n(t)
                }
            }).on("typeahead:closed", function (e, t) {
                for (var o = !1, n = 0; n < r.length && !o; n++) r[n].DisplayText === i.val() && (o = !0);
                if (0 == l || "" == i.val() || r.length > 0 && 0 == o) return i.typeahead("val", ""), i.prop("title", ""), a.val("0"), !1
            }).bind("keypress", function (e) {
                var t = new RegExp("^[a-zA-Z0-9\b :.,_-]+$"),
                    a = String.fromCharCode(e.charCode ? e.charCode : e.which);
                return !(9 != e.keyCode && !e.ctrlKey && 86 != e.keyCode && 88 != e.keyCode && 46 != e.keyCode && 37 != e.keyCode && 39 != e.keyCode) || (t.test(a) ? void 0 : (e.preventDefault(), !1))
            }).bind("paste input", RemoveAutoSpecialChar), i
    }
}), jQuery.fn.extend({
    typeheadService: function (e, t, a, o) {
        $(this).attr("type", "Search");
        var n = $(this),
            i = [],
            r = !0;
        return n.attr("title", n.val()), n.keypress(function (e) {
            if (13 == (e.keyCode || e.which) || 34 == (e.keyCode || e.which) || 39 == (e.keyCode || e.which)) return e.preventDefault(), !1
        }), n.typeahead({
            hint: !1,
            highlight: !1,
            minLength: 0,
            maxItem: 20,
            order: "asc"
        }, {
                name: "ID",
                displayKey: "DisplayText",
                source: function (t, o) {
                    var l = "";
                    l = "G" == e.attr("d-book-type") || "Q" == e.attr("d-book-type") ? e.find("input.hdnConfigID").val().replace(new RegExp(",,", "g"), ",") : "A" == e.find("input.hdnPageSrvType").val() ? e.find("input.hdnConfigID").val().replace(new RegExp(",,", "g"), ",") : e.find("input.hdnPaxConfigID").val().replace(new RegExp(",,", "g"), ",");
                    var s = {
                        bResrvID: a,
                        bdate: ForamtdatetoServer(e.find("input.txtBDate")),
                        bCityID: Number(e.find("input.hdnCity").val()),
                        bPartnerID: Number(e.find("input.hdnPartner").val()),
                        bSrvTypID: Number(e.find("input.hdnSrvType").val()),
                        bFZoneID: Number(e.find("input.hdnZoneFrom").val()),
                        bTZoneID: Number(e.find("input.hdnZoneTo").val()),
                        bQty: Number(e.find(".txtQty").val()),
                        SearchText: n.val(),
                        bConfigID: l,
                        BookID: "Q" == e.attr("d-book-type") ? Number(e.attr("d-tpr_pckg_id")) : Number(e.attr("d-Book_ID")),
                        bType: e.attr("d-book-type")
                    };
                    "A" == e.find("input.hdnPageSrvType").val() && 0 == Number(e.find("input.hdnCity").val()) ? (requiredAutoComplete(e.find("input.tt-input.txtCity"), e.find("input.hdnCity"), "City cannot left blank") || e.find("input.tt-input.txtCity").setCustomError("City cannot left blank"), requiredAutoComplete(e.find("input.tt-input.txtPartner"), e.find("input.hdnPartner"), "Partner cannot left blank") || e.find("input.tt-input.txtPartner").setCustomError("Partner cannot left blank")) : ("../Private/GetbookingServiceData", $.ajax({
                        url: "../Private/GetbookingServiceData",
                        dataType: "json",
                        type: "POST",
                        data: {
                            objParam: s
                        },
                        success: function (e) {
                            if (i = [], $.each(e, function (e, t) {
                                i.push({
                                    ID: t.ID,
                                    Code: t.Code,
                                    DisplayText: "<span>" + (t.Name != null ? t.Name.split('$')[0] : t.Name) + '</span><span style="float:right">' + (t.Name != null ? t.Name.split('$')[1] : t.Name) + '</span><span class="spntime" style="display:none;">' + (t.Name != null ? t.Name.split('$')[2] : t.Name) + "</span>"
                                })
                            }), $(window).width() > 767)
                                if (n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").removeClass("btm_top"), n.closest("table.smallTabTwitter").length > 0) {
                                    var t = n.offset().left - n.closest("table.smallTabTwitter").offset().left,
                                        a = n.offset().top - n.closest("table.smallTabTwitter").offset().top + n.height();
                                    n.closest("span.twitter-typeahead").css("position", "static"), n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").css("top", a).css("left", t).css("width", n.width())
                                } else {
                                    if (n.closest("div.mCSB_container").length && (n.closest("div.mCSB_container").css("overflow", "hidden"), Number(n.closest("tbody").height()) > Number(n.closest("div.mCSB_container").height()) ? n.closest("div.mCSB_container").css("overflow", "visible") : n.closest("div.mCSB_container").css("overflow", "hidden")), n.closest("tbody").length > 0) n.offset().top - n.closest("tbody").offset().top > 200 && n.offset().top + n.outerHeight() > $(window).innerHeight() / 2 && n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                                    else n.offset().top + n.outerHeight() > $(window).innerHeight() / 2 && n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top")
                                } else n.offset().top + n.outerHeight() > $(window).innerHeight() / 2 && n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                            r = 0 != i.length, o(i)
                        },
                        error: function (e) {
                            swalValidationAlert(e.responseText, "error")
                        },
                        failure: function (e) {
                            swalValidationAlert(e.responseText, "error")
                        }
                    }))
                },
                templates: {
                    empty: '<span class="TH_NoRecordFound">No Record Found<span>'
                }
            }).on("typeahead:selected", function (e, a) {
                if (t.val(a.ID), n.removeClass("error"), n.prop("title", a.Code.split("$")[1]), n.removeAttr("data-original-title"), o && "function" == typeof o) {
                    var i = n.attr("d-typehead");
                    i && null != i ? (o(a, i, n), n.attr("data-original-title", "")) : o(a)
                }
            }).on("typeahead:autocompleted", function (e, a) {
                if (t.val(a.ID), n.prop("title", a.Code.split("$")[1]), o && "function" == typeof o) {
                    var i = n.attr("d-typehead");
                    i && null != i ? o(a, i) : o(a)
                }
            }).on("typeahead:closed", function (e, a) {
                for (var o = !1, l = 0; l < i.length && !o; l++) i[l].Code.split("$")[1] === n.val() && (o = !0);
                if (0 == r || "" == n.val() || i.length > 0 && 0 == o) return n.typeahead("val", ""), n.prop("title", ""), t.val("0"), !1
            }).bind("keypress", function (e) {
                var t = new RegExp("^[a-zA-Z0-9\b :.,_-]+$"),
                    a = String.fromCharCode(e.charCode ? e.charCode : e.which);
                return !(9 != e.keyCode && !e.ctrlKey && 86 != e.keyCode && 88 != e.keyCode && 46 != e.keyCode && 37 != e.keyCode && 39 != e.keyCode) || (t.test(a) ? void 0 : (e.preventDefault(), !1))
            }).bind("paste input", RemoveAutoSpecialChar), n
    }
}), jQuery.fn.extend({
    typeheadWithoutCityService: function (e, t, a, o) {
        $(this).attr("type", "Search");
        var n = $(this),
            i = [],
            r = !0;
        return n.attr("title", n.val()), n.keypress(function (e) {
            if (13 == (e.keyCode || e.which) || 34 == (e.keyCode || e.which) || 39 == (e.keyCode || e.which)) return e.preventDefault(), !1
        }), n.typeahead({
            hint: !1,
            highlight: !1,
            minLength: 0,
            maxItem: 20,
            order: "asc"
        }, {
            name: "ID",
            displayKey: "DisplayText",
            source: function (t, o) {
                var l = "";
                l = "G" == e.attr("d-book-type") || "Q" == e.attr("d-book-type") ? e.find("input.hdnConfigID").val().replace(new RegExp(",,", "g"), ",") : "A" == e.find("input.hdnPageSrvType").val() ? e.find("input.hdnConfigID").val().replace(new RegExp(",,", "g"), ",") : e.find("input.hdnPaxConfigID").val().replace(new RegExp(",,", "g"), ",");
                var s = {
                    bResrvID: a,
                    bdate: ForamtdatetoServer(e.find("input.txtBDate")),
                    bCityID: Number(e.find("input.hdnCity").val()),
                    bPartnerID: Number(e.find("input.hdnPartner").val()),
                    bSrvTypID: Number(e.find("input.hdnSrvType").val()),
                    bFZoneID: Number(e.find("input.hdnZoneFrom").val()),
                    bTZoneID: Number(e.find("input.hdnZoneTo").val()),
                    bQty: Number(e.find(".txtQty").val()),
                    SearchText: n.val(),
                    bConfigID: l,
                    BookID: "Q" == e.attr("d-book-type") ? Number(e.attr("d-tpr_pckg_id")) : Number(e.attr("d-Book_ID")),
                    bType: e.attr("d-book-type")
                };
                "A" == e.find("input.hdnPageSrvType").val() && 0 == Number(e.find("input.hdnPartner").val()) ? (requiredAutoComplete(e.find("input.tt-input.txtPartner"), e.find("input.hdnPartner"), "Partner cannot left blank") || e.find("input.tt-input.txtPartner").setCustomError("Partner cannot left blank")) : ("../Private/GetbookingServiceData", $.ajax({
                    url: "../Private/GetbookingServiceData",
                    dataType: "json",
                    type: "POST",
                    data: {
                        objParam: s
                    },
                    success: function (e) {
                        if (i = [], $.each(e, function (e, t) {
                            i.push({
                                ID: t.ID,
                                Code: t.Code,
                                DisplayText: "<span>" + (t.Name != null ? t.Name.split('$')[0] : t.Name) + '</span><span style="float:right">' + (t.Name != null ? t.Name.split('$')[1] : t.Name) + '</span><span class="spntime" style="display:none;">' + (t.Name != null ? t.Name.split('$')[2] : t.Name) + "</span>"
                            })
                        }), $(window).width() > 767)
                            if (n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").removeClass("btm_top"), n.closest("table.smallTabTwitter").length > 0) {
                                var t = n.offset().left - n.closest("table.smallTabTwitter").offset().left,
                                    a = n.offset().top - n.closest("table.smallTabTwitter").offset().top + n.height();
                                n.closest("span.twitter-typeahead").css("position", "static"), n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").css("top", a).css("left", t).css("width", n.width())
                            } else {
                                if (n.closest("div.mCSB_container").length && (n.closest("div.mCSB_container").css("overflow", "hidden"), Number(n.closest("tbody").height()) > Number(n.closest("div.mCSB_container").height()) ? n.closest("div.mCSB_container").css("overflow", "visible") : n.closest("div.mCSB_container").css("overflow", "hidden")), n.closest("tbody").length > 0) n.offset().top - n.closest("tbody").offset().top > 200 && n.offset().top + n.outerHeight() > $(window).innerHeight() / 2 && n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                                else n.offset().top + n.outerHeight() > $(window).innerHeight() / 2 && n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top")
                            } else n.offset().top + n.outerHeight() > $(window).innerHeight() / 2 && n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                        r = 0 != i.length, o(i)
                    },
                    error: function (e) {
                        swalValidationAlert(e.responseText, "error")
                    },
                    failure: function (e) {
                        swalValidationAlert(e.responseText, "error")
                    }
                }))
            },
            templates: {
                empty: '<span class="TH_NoRecordFound">No Record Found<span>'
            }
        }).on("typeahead:selected", function (e, a) {
            if (t.val(a.ID), n.removeClass("error"), n.prop("title", a.Code.split("$")[1]), n.removeAttr("data-original-title"), o && "function" == typeof o) {
                var i = n.attr("d-typehead");
                i && null != i ? (o(a, i, n), n.attr("data-original-title", "")) : o(a)
            }
        }).on("typeahead:autocompleted", function (e, a) {
            if (t.val(a.ID), n.prop("title", a.Code.split("$")[1]), o && "function" == typeof o) {
                var i = n.attr("d-typehead");
                i && null != i ? o(a, i) : o(a)
            }
        }).on("typeahead:closed", function (e, a) {
            for (var o = !1, l = 0; l < i.length && !o; l++) i[l].Code.split("$")[1] === n.val() && (o = !0);
            if (0 == r || "" == n.val() || i.length > 0 && 0 == o) return n.typeahead("val", ""), n.prop("title", ""), t.val("0"), !1
        }).bind("keypress", function (e) {
            var t = new RegExp("^[a-zA-Z0-9\b :.,_-]+$"),
                a = String.fromCharCode(e.charCode ? e.charCode : e.which);
            return !(9 != e.keyCode && !e.ctrlKey && 86 != e.keyCode && 88 != e.keyCode && 46 != e.keyCode && 37 != e.keyCode && 39 != e.keyCode) || (t.test(a) ? void 0 : (e.preventDefault(), !1))
        }).bind("paste input", RemoveAutoSpecialChar), n
    }
}),jQuery.fn.extend({
    typeheadAutoCompleteWithMultipleValue: function (e, t, a, o, n) {
        var i = $(this),
            r = [],
            l = !0;
        return i.attr("title", i.val()), i.keypress(function (e) {
            if (13 == (e.keyCode || e.which)) return e.preventDefault(), !1
        }), i.typeahead({
            hint: !1,
            highlight: !1,
            minLength: 0,
            maxItem: 20,
            order: "asc"
        }, {
                name: "ID",
                displayKey: "DisplayText",
                source: function (a, n) {
                    var s, d = {};
                    d.Table = e, d.Prefix = t, d.SearchText = i.val();
                    var c = "";
                    if (o && null != o)
                        for (var p = 0; p < o.length; p++) {
                            var u = $("#" + o[p].CtrName).val();
                            u.length > 0 && (c += " and " + o[p].ParamName, u.indexOf(",") > 0 ? c += " in (" + u + ")" : c += (o[p].opr && 1 == o[p].opr ? "!" : "") + "=" + ($.isNumeric(u) ? Number(u) : "'" + u + "'"))
                        }
                    c.length > 0 ? (s = SERVICEPATH + "ws_GetSuggestedValueWithCondition", d.WhereCond = c) : s = SERVICEPATH + "ws_GetSuggestedValue", $.ajax({
                        url: s,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(d),
                        success: function (e) {
                            r = [], $.each(e, function (e, t) {
                                r.push({
                                    ID: t.ID,
                                    Code: t.Code,
                                    DisplayText: t.Name
                                })
                            }), i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").removeClass("btm_top"), i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top"), l = 0 != r.length, n(r)
                        },
                        error: function (e) {
                            swalValidationAlert("", "error", e.responseText)
                        },
                        failure: function (e) {
                            swalValidationAlert("", "error", e.responseText)
                        }
                    })
                },
                templates: {
                    empty: '<span class="TH_NoRecordFound">No Record Found<span>'
                }
            }).on("typeahead:selected", function (e, t) {
                if (a.val(t.ID), i.removeClass("error"), i.attr("title", t.DisplayText), n && "function" == typeof n) {
                    var o = i.attr("d-typehead");
                    o && null != o ? n(t, o) : n(t)
                }
            }).on("typeahead:autocompleted", function (e, t) {
                if (a.val(t.ID), i.attr("title", t.DisplayText), n && "function" == typeof n) {
                    var o = i.attr("d-typehead");
                    o && null != o ? n(t, o) : n(t)
                }
            }).on("typeahead:closed", function (e, t) {
                for (var o = !1, n = 0; n < r.length && !o; n++) r[n].DisplayText === i.val() && (o = !0);
                if (0 == l || "" == i.val() || r.length > 0 && 0 == o) return i.typeahead("val", ""), i.prop("title", ""), a.val("0"), !1
            }).bind("keypress", function (e) {
                var t = new RegExp("^[a-zA-Z0-9\b? :,_-]+$"),
                    a = String.fromCharCode(e.charCode ? e.charCode : e.which);
                return 9 == e.keyCode || (t.test(a) ? void 0 : (e.preventDefault(), !1))
            }).bind("paste input", RemoveAutoSpecialChar), i
    }
}), jQuery.fn.extend({
    WithChildAutoComplete: function (e, t, a, o, n) {
        $(this).attr("type", "Search");
        var i = $(this),
            r = [],
            l = !0;
        return i.attr("title", i.val()), i.keypress(function (e) {
            if (13 == (e.keyCode || e.which) || 34 == (e.keyCode || e.which) || 39 == (e.keyCode || e.which)) return e.preventDefault(), !1
        }), i.typeahead({
            hint: !1,
            highlight: !1,
            minLength: 0,
            maxItem: 20,
            order: "asc"
        }, {
                name: "ID",
                displayKey: "DisplayText",
                source: function (a, n) {
                    var s, d = {};
                    d.Table = e, d.Prefix = t, d.SearchText = i.val();
                    var c = "";
                    if (o && null != o)
                        for (var p = 0; p < o.length; p++) {
                            var u = $("#" + o[p].CtrName).val();
                            u.length > 0 && (c += " and " + o[p].ParamName, c += (o[p].opr && 1 == o[p].opr ? "!" : "") + "=" + ($.isNumeric(u) ? Number(u) : "'" + u + "'"))
                        }
                    c.length > 0 ? (s = SERVICEPATH + "ws_GetSuggestedValueWithChild", d.WhereCond = c) : (s = SERVICEPATH + "ws_GetSuggestedValueWithChild", d.WhereCond = ""), $.ajax({
                        url: s,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(d),
                        success: function (e) {
                            r = [], $.each(e, function (e, t) {
                                r.push({
                                    ID: t.ID,
                                    ChildName: t.ChildName,
                                    DisplayText: t.Name,
                                    ChildID: t.ChildID,
                                    Code: t.Code // Change on 20092024 for Added ToCity for rail service By Prajapati Revtaram
                                })
                            }), i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").removeClass("btm_top"), i.offset().top + i.outerHeight() > $(window).innerHeight() / 2 && i.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top"), l = 0 != r.length, n(r)
                        },
                        error: function (e) {
                            swalValidationAlert("", "error", e.responseText)
                        },
                        failure: function (e) {
                            swalValidationAlert("", "error", e.responseText)
                        }
                    })
                },
                templates: {
                    empty: '<span class="TH_NoRecordFound">No Record Found<span>'
                }
            }).on("typeahead:selected", function (e, t) {
                if (a.val(t.ID), i.removeClass("error"), i.attr("title", t.DisplayText), i.removeAttr("data-original-title"), n && "function" == typeof n) {
                    var o = i.attr("d-typehead");
                    o && null != o ? (n(t, o), i.attr("data-original-title", "")) : n(t)
                }
            }).on("typeahead:autocompleted", function (e, t) {
                if (a.val(t.ID), i.attr("title", t.DisplayText), n && "function" == typeof n) {
                    var o = i.attr("d-typehead");
                    o && null != o ? n(t, o) : n(t)
                }
            }).on("typeahead:closed", function (e, t) {
                for (var o = !1, n = 0; n < r.length && !o; n++) r[n].DisplayText === i.val() && (o = !0);
                if (0 == l || "" == i.val() || r.length > 0 && 0 == o) return i.typeahead("val", ""), i.prop("title", ""), a.val("0"), !1
            }).bind("keypress", function (e) {
                var t = new RegExp("^[a-zA-Z0-9\b? :,_-]+$"),
                    a = String.fromCharCode(e.charCode ? e.charCode : e.which);
                return !(9 != e.keyCode && !e.ctrlKey && 86 != e.keyCode && 88 != e.keyCode && 46 != e.keyCode && 37 != e.keyCode && 39 != e.keyCode) || (t.test(a) ? void 0 : (e.preventDefault(), !1))
            }).bind("paste input", RemoveAutoSpecialChar), i
    }
}), jQuery.fn.extend({
    MultiselectAutoComplete: function (e, t, a, o) {
        $(this).attr("type", "Search");
        var n = $(this),
            i = !0,
            r = [];
        return n.keypress(function (e) {
            if (13 == (e.keyCode || e.which)) return e.preventDefault(), !1
        }), n.typeahead({
            hint: !1,
            highlight: !1,
            minLength: 0,
            maxItem: 20,
            order: "asc"
        }, {
                name: "ID",
                displayKey: "DisplayText",
                source: function (o, l) {
                    var s, d = {};
                    d.Table = e, d.Prefix = t, d.SearchText = n.val();
                    var c = "";
                    if (a && null != a)
                        for (var p = 0; p < a.length; p++) {
                            var u = $("#" + a[p].CtrName).val();
                            u.length > 0 && (c += " and " + a[p].ParamName, c += (a[p].opr && 1 == a[p].opr ? "!" : "") + "=" + ($.isNumeric(u) ? Number(u) : "'" + u + "'"))
                        }
                    c.length > 0 ? (s = SERVICEPATH + "ws_GetSuggestedValueWithChild", d.WhereCond = c) : (s = SERVICEPATH + "ws_GetSuggestedValueWithChild", d.WhereCond = ""), $.ajax({
                        url: s,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(d),
                        success: function (e) {
                            (r = [], $.each(e, function (e, t) {
                                r.push({
                                    ID: t.ID,
                                    DisplayText: t.Name
                                })
                            }), $(window).width() > 767) ? (n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").removeClass("btm_top"), n.closest("div.mCSB_container").length && (n.closest("div.mCSB_container").css("overflow", "hidden"), Number(n.closest("tbody").height()) > Number(n.closest("div.mCSB_container").height()) ? n.closest("div.mCSB_container").css("overflow", "visible") : n.closest("div.mCSB_container").css("overflow", "hidden")), n.closest("tbody").length > 0 ? n.offset().top - n.closest("tbody").offset().top > 200 && n.offset().top + n.outerHeight() > $(window).innerHeight() / 2 && n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top") : n.offset().top + n.outerHeight() > $(window).innerHeight() / 2 && n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top")) : n.offset().top + nd.outerHeight() > $(window).innerHeight() / 2 && n.closest("span.twitter-typeahead").find(".tt-dropdown-menu").addClass("btm_top");
                            i = 0 != r.length, l(r)
                        },
                        error: function (e) {
                            swalValidationAlert("", "error", e.responseText)
                        },
                        failure: function (e) {
                            swalValidationAlert("", "error", e.responseText)
                        }
                    })
                },
                templates: {
                    empty: '<span class="TH_NoRecordFound">No Record Found<span>'
                }
            }).on("typeahead:selected", function (e, t) {
                if (n.closest("span.twitter-typeahead").siblings("ul#ULSelectedAddons").find('li[d-val="' + t.ID + '"]').length > 0) {
                    var a = n.closest("span.twitter-typeahead").siblings("ul#ULSelectedAddons").find("li").length;
                    n.typeahead("val", a + " Items selected").val(a + " Items selected"), swalValidationAlert("Selected Item is already added in list.", "error", "Please select any other item.")
                } else {
                    n.closest("span.twitter-typeahead").siblings("ul#ULSelectedAddons").append('<li d-val="' + t.ID + '"><span id="spnDelete' + t.ID + '" ><i class="icon-cross text-danger-400" title="Remove"></i></span>' + t.DisplayText + "</li>"), n.closest("span.twitter-typeahead").siblings("ul#ULSelectedAddons").find('li span[id="spnDelete' + t.ID + '"]').click(function () {
                        var e = $(this).closest("li"),
                            t = $(this).closest("ul").find("li").length - 1;
                        return e.closest("ul").siblings(".twitter-typeahead").find('input[id="' + n.attr("id") + '"]').typeahead("val", t + " Items selected").val(t + " Items selected"), e.fadeOut(400, function () {
                            e.remove()
                        }), !0
                    });
                    a = n.closest("span.twitter-typeahead").siblings("ul#ULSelectedAddons").find("li").length;
                    n.typeahead("val", a + " Items selected").val(a + " Items selected")
                }
                if (o && "function" == typeof o) {
                    var i = n.attr("d-typehead");
                    i && null != i ? o(t, i) : o(t)
                }
            }).on("typeahead:autocompleted", function (e, t) {
                if (o && "function" == typeof o) {
                    var a = n.attr("d-typehead");
                    a && null != a ? o(t, a) : o(t)
                }
            }).on("typeahead:closed", function (e, t) {
                if (0 == i && n.val().indexOf("Items selected") <= 0 && r.length >= 0) return n.typeahead("val", ""), n.prop("title", ""), !1
            }).bind("keypress", function (e) {
                var t = new RegExp("^[a-zA-Z0-9\b? :,_-]+$"),
                    a = String.fromCharCode(e.charCode ? e.charCode : e.which);
                return 9 == e.keyCode || (t.test(a) ? void 0 : (e.preventDefault(), !1))
            }).bind("paste input", RemoveAutoSpecialChar), n
    }
}), jQuery.fn.extend({
    typeheadMultiSelect: function (e, t, a, o, n, i) {
        var r = "";
        if (i && null != i)
            for (var l = 0; l < i.length; l++) {
                var s = $("#" + i[l].CtrName).val();
                s.length > 0 && (r += " and " + i[l].ParamName, r += (i[l].opr && 1 == i[l].opr ? "!" : "") + "=" + ($.isNumeric(s) ? Number(s) : "'" + s + "'"))
            }
        $(this).select2({
            minimumInputLength: n,
            allowClear: !0,
            multiple: !0,
            theme: "classic",
            maximumInputLength: 20,
            ajax: {
                quietMillis: 150,
                url: SERVICEPATH + "ws_GetMulitSelectSuggestedValue",
                dataType: "json",
                data: function (a, n) {
                    return {
                        pageSize: o,
                        pageNum: n,
                        SearchText: a,
                        table: e,
                        prefix: t,
                        WhereCond: r
                    }
                },
                results: function (e, t) {
                    var a = t * o < e.Total;
                    return {
                        results: e.Results,
                        more: a
                    }
                }
            },
            delay: 50,
            cache: !0,
            closeOnSelect: !1
        }).on("change", function (e) {
            $('#s2id_autogen2').val('');
            a.val(e.val), $(".select2-input").attr("title", "")
        }).bind("keypress", function (e) {
            var t = new RegExp("^[a-zA-Z0-9\b? :,_-]+$"),
                a = String.fromCharCode(e.charCode ? e.charCode : e.which);
            return 9 == e.keyCode || (t.test(a) ? void 0 : (e.preventDefault(), !1))
        }).bind("paste input", RemoveAutoSpecialChar)
    }
}), jQuery.fn.extend({
    FlexAutoComplete: function (e, t, a) {
        $(this).attr("type", "Search");
        var o = $(this),
            n = [],
            i = !0;
        return o.attr("title", o.val()), o.keypress(function (e) {
            if (13 == (e.keyCode || e.which) || 34 == (e.keyCode || e.which) || 39 == (e.keyCode || e.which)) return e.preventDefault(), !1
        }), o.typeahead({
            hint: !1,
            highlight: !1,
            minLength: 0,
            order: "asc"
        }, {
                name: "ID",
                displayKey: "DisplayText",
                source: function (a, r) {
                    var l, s = {};
                    s.Module = e, s.Table = t, s.SearchText = o.val(), l = SERVICEPATH + "ws_GetSuggestedFlex", $.ajax({
                        url: l,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(s),
                        success: function (e) {
                            n = [], $.each(e, function (e, t) {
                                n.push({
                                    ID: t.ID,
                                    DisplayText: t.Text
                                })
                            }), i = 0 != n.length, r(n)
                        },
                        error: function (e) {
                            swalValidationAlert("", "error", e.responseText)
                        },
                        failure: function (e) {
                            swalValidationAlert("", "error", e.responseText)
                        }
                    })
                },
                templates: {
                    empty: '<span class="TH_NoRecordFound">No Record Found<span>'
                }
            }).on("typeahead:selected", function (e, t) {
                a.val(t.ID), o.removeClass("error"), o.attr("title", t.DisplayText)
            }).on("typeahead:autocompleted", function (e, t) {
                a.val(t.ID), o.attr("title", t.DisplayText)
            }).on("typeahead:closed", function (e, t) {
                for (var r = !1, l = 0; l < n.length && !r; l++) n[l].DisplayText === o.val() && (r = !0);
                if (0 == i || "" == o.val() || n.length > 0 && 0 == r) return o.typeahead("val", ""), o.prop("title", ""), a.val("0"), !1
            }).bind("keypress", function (e) {
                var t = new RegExp("^[a-zA-Z0-9\b :.,_-]+$"),
                    a = String.fromCharCode(e.charCode ? e.charCode : e.which);
                return !(9 != e.keyCode && !e.ctrlKey && 86 != e.keyCode && 88 != e.keyCode && 46 != e.keyCode && 37 != e.keyCode && 39 != e.keyCode) || (t.test(a) ? void 0 : (e.preventDefault(), !1))
            }).bind("paste input", RemoveAutoSpecialChar), o
    }
}), FileHandling = {
    Upload: function (e, t, a, o, n) {
        var i = e.attr("id"),
            r = new FormData;
        void 0 === n && (n = !0);
        document.getElementById(i).files.length;
        if (document.getElementById(i).files.length > 0) {
            var l = document.getElementById(i).files[0];
            r.append("file", l), r.append("folderpath", t), r.append("filename", a), $.ajax({
                type: "POST",
                async: n,
                url: SERVICEPATH + "ws_FileUpload",
                data: r,
                dataType: "json",
                contentType: !1,
                processData: !1,
                success: function () {
                    o("success")
                },
                error: function (e) {
                    o("error", e)
                },
                failure: function (e) {
                    o("failure", e)
                }
            })
        }
    },
    Download: function (e, t, a) {
        var o = new FormData;
        t.length > 0 && (o.append("folderpath", e), o.append("filename", t), $.ajax({
            type: "POST",
            url: SERVICEPATH + "ws_FileDownload",
            data: o,
            dataType: "json",
            contentType: !1,
            processData: !1,
            success: function (e) {
                a(e)
            },
            error: function (e) {
                a("error", e)
            },
            failure: function (e) {
                a("failure", e)
            }
        }))
    }
}, Check_Existance = function (e, t, a, o, n) {
    $(e).on("change", function () {
        var i, r = {};
        r.Table = t, r.Prefix = a, r.SearchText = "";
        var l = "";
        if (o && null != o)
            for (var s = 0; s < o.length; s++) {
                var d = $("#" + o[s].CtrName).val();
                d.length > 0 && (l += " and " + o[s].ParamName, l += (o[s].opr && 1 == o[s].opr ? "!" : "") + "='" + d + "'")
            }
        i = SERVICEPATH + "ws_GetSuggestedValueWithCondition", r.WhereCond = l;
        $.ajax({
            url: i,
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(r),
            success: function (t) {
                t.length > 0 ? ($(e).setCustomError(n), $(e).val("")) : $("#txtReference").clearCustomError()
            }
        })
    })
}, Check_RefExistance = function (e, t, a, o, n) {
    var i, r = {};
    r.Table = t, r.Prefix = a, r.SearchText = "";
    var l = !0,
        s = "";
    if (o && null != o)
        for (var d = 0; d < o.length; d++) {
            var c = $("#" + o[d].CtrName).val();
            c.length > 0 && (s += " and " + o[d].ParamName, s += (o[d].opr && 1 == o[d].opr ? "!" : "") + "='" + c + "'")
        }
    i = SERVICEPATH + "ws_GetSuggestedValueWithCondition", r.WhereCond = s;
    return $.ajax({
        url: i,
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(r),
        success: function (t) {
            t.length > 0 ? ($(e).setCustomError(n), $(e).val(""), l = !1) : ($("#txtReference").clearCustomError(), l = !0)
        }
    }), l
}, Populate_Date = function (e, t) {
    var a = DateDisplay,
        o = DateEntry;
    $(e).datepicker({
        dateFormat: a,
        showOn: "button",
        buttonImageOnly: !0,
        showOtherMonths: !0,
        selectOtherMonths: !0,
        changeMonth: !0,
        changeYear: !0,
        showWeek: !0,
        showAnim: "fadeIn",
        yearRange: "-10:+60",
        onSelect: function () {
            $(e).change()
        }
    }), $(e).closest("div").find("img.ui-datepicker-trigger").attr("title", $(e).attr("placeholder")), $(e).on("change", function () {
        CheckVailidDate(e, a, o), t && t(ForamtdatetoServer(e))
    }), $(".ui-datepicker-trigger").attr("alt", ""), $(e).on("click focusin", function () {
        $('[data-toggle="tooltip"]').bstooltip("hide")
    }), $(".ui-datepicker-trigger").on("click focusin", function () {
        $('[data-toggle="tooltip"]').bstooltip("hide")
    })
}, Populate_Date_Dynamic_Year = function (e, t, a, o) {
    var n = DateDisplay,
        i = DateEntry;
    $(e).datepicker({
        dateFormat: n,
        showOn: "button",
        buttonImageOnly: !0,
        showOtherMonths: !0,
        selectOtherMonths: !0,
        changeMonth: !0,
        changeYear: !0,
        maxDate: a,
        showWeek: !0,
        showAnim: "fadeIn",
        yearRange: t,
        onSelect: function () {
            $(e).change()
        }
    }), $(e).closest("div").find("img.ui-datepicker-trigger").attr("title", $(e).attr("placeholder")), $(e).on("change", function () {
        CheckVailidDate(e, n, i), o && o(ForamtdatetoServer(e))
    }), $(".ui-datepicker-trigger").attr("alt", ""), $(e).on("click focusin", function () {
        $('[data-toggle="tooltip"]').bstooltip("hide")
    }), $(".ui-datepicker-trigger").on("click focusin", function () {
        $('[data-toggle="tooltip"]').bstooltip("hide")
    })
}, Populate_Date_Restrict_previous = function (e, t, a, o) {
    var n = DateDisplay,
        i = DateEntry;
    $(e).datepicker({
        dateFormat: n,
        showOn: "button",
        buttonImageOnly: !0,
        showOtherMonths: !0,
        selectOtherMonths: !0,
        changeMonth: !0,
        changeYear: !0,
        maxDate: t,
        showWeek: !0,
        showAnim: "fadeIn",
        yearRange: a,
        onSelect: function () {
            $(e).change()
        }
    }), $(e).closest("div").find("img.ui-datepicker-trigger").attr("title", $(e).attr("placeholder")), $(e).on("change", function () {
        CheckVailidDate(e, n, i), o && o(ForamtdatetoServer(e))
    }), $(".ui-datepicker-trigger").attr("alt", ""), $(e).on("click focusin", function () {
        $('[data-toggle="tooltip"]').bstooltip("hide")
    }), $(".ui-datepicker-trigger").on("click focusin", function () {
        $('[data-toggle="tooltip"]').bstooltip("hide")
    })
}, Populate_Date_Restrict = function (e, t, a) {
    var o = DateDisplay,
        n = DateEntry;
    $(e).datepicker({
        dateFormat: o,
        showOn: "button",
        buttonImageOnly: !0,
        showOtherMonths: !0,
        selectOtherMonths: !0,
        changeMonth: !0,
        changeYear: !0,
        minDate: t,
        showWeek: !0,
        showAnim: "fadeIn",
        yearRange: "-10:+40",
        onSelect: function () {
            $(e).change()
        }
    }), $(e).closest("div").find("img.ui-datepicker-trigger").attr("title", $(e).attr("placeholder")), $(e).on("change", function () {
        CheckVailidDate(e, o, n), a && a(ForamtdatetoServer(e))
    }), $(".ui-datepicker-trigger").attr("alt", ""), $(e).on("click focusin", function () {
        $('[data-toggle="tooltip"]').bstooltip("hide")
    }), $(".ui-datepicker-trigger").on("click focusin", function () {
        $('[data-toggle="tooltip"]').bstooltip("hide")
    })
}, CompareDate = function (e, t, a) {
    var o = DateDisplay;
    return "" != $(e).val() && "" != $(e).val() && $.datepicker.parseDate(o, $(e).val()) > $.datepicker.parseDate(o, $(t).val()) ? ($(e).addClass("error"), $(e).attr("data-toggle", "tooltip"), $(e).attr("data-original-title", a), $(e).attr("title", a), $(t).addClass("error"), $(t).attr("data-toggle", "tooltip"), $(t).attr("title", a), $(t).attr("data-original-title", a), $('[data-toggle="tooltip"]').bstooltip({
        placement: "bottom"
    }), !1) : ($(e).removeClass("error"), $(e).removeAttr("data-toggle"), $(e).removeAttr("data-original-title"), $(e).removeAttr("title"), $(t).removeClass("error"), $(t).removeAttr("data-toggle"), $(t).removeAttr("title"), $(t).removeAttr("data-original-title"), !0)
}, CompareDateWithVariable = function (e, t, a) {
    var o = DateDisplay;
    if ("" != $(e).val() && "" != $(e).val()) return $.datepicker.parseDate(o, $(e).val()) <= $.datepicker.parseDate(o, t) ? ($(e).addClass("error"), $(e).attr("data-toggle", "tooltip"), $(e).attr("data-original-title", a), $(e).attr("title", a), $('[data-toggle="tooltip"]').bstooltip({
        placement: "top"
    }), !1) : ($(e).removeClass("error"), $(e).removeAttr("data-toggle"), $(e).removeAttr("data-original-title"), $(e).removeAttr("title"), !0)
    },
    Populate_Date_RestrictRange = function (ctl_TextBox, dateToday, dateMaxday, fn_callback) {
        var DFormat = DateDisplay;
        var EFormat = DateEntry;
        $(ctl_TextBox).datepicker({
            dateFormat: DFormat,
            showOn: "button",
            buttonImageOnly: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            changeMonth: true,
            changeYear: true,
            minDate: dateToday,
            maxDate: dateMaxday,
            showWeek: true,
            showAnim: "fadeIn",
            yearRange: "-10:+40",
            onSelect: function () {
                $(ctl_TextBox).change();
            },
        });
        $(ctl_TextBox).closest('div').find('img.ui-datepicker-trigger').attr('title', $(ctl_TextBox).attr('placeholder'));
        $(ctl_TextBox).on('change', function () {
            CheckVailidDate(ctl_TextBox, DFormat, EFormat)
            if (!!fn_callback) {
                fn_callback(ForamtdatetoServer(ctl_TextBox))
            }
        });
        $('.ui-datepicker-trigger').attr('alt', '');
        $(ctl_TextBox).on('click focusin', function () {
            $('[data-toggle="tooltip"]').bstooltip('hide');
        });
        $('.ui-datepicker-trigger').on('click focusin', function () {
            $('[data-toggle="tooltip"]').bstooltip('hide');
        });
    }
    ;
$(function () {
    $(document).click(function (e) {
        var container2 = $(".twitter-typeahead");
        if (!container2.is(e.target) && container2.has(e.target).length === 0) {
            $(this).find('.tt-dropdown-menu').removeClass("tt-top");
            $(this).find('.tt-dropdown-menu').removeClass("tt-bottom");
        }
    });
    $('.twitter-typeahead').find('.tt-dropdown-menu').removeClass("tt-top");
    $('.twitter-typeahead').find('.tt-dropdown-menu').removeClass("tt-bottom");
    setTimeout(function () {
        $('.twitter-typeahead .form-control').click(function () {
            if ($(this).offset().top + $(this).outerHeight() - $(window).scrollTop() > ($(window).innerHeight() / 2)) {
                $(this).parent().find('.tt-dropdown-menu').addClass("tt-top");
                $(this).parent().find('.tt-dropdown-menu').removeClass("tt-bottom");
            } else {
                $(this).parent().find('.tt-dropdown-menu').removeClass("tt-top");
                $(this).parent().find('.tt-dropdown-menu').addClass("tt-bottom");
            }
        });
    }, 100);
});