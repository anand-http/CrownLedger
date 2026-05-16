! function (t) {
    var e = {
        isMsie: function () {
            return !!/(msie|trident)/i.test(navigator.userAgent) && navigator.userAgent.match(/(msie |rv:)(\d+(.\d+)?)/i)[2]
        },
        isBlankString: function (t) {
            return !t || /^\s*$/.test(t)
        },
        escapeRegExChars: function (t) {
            return t.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&")
        },
        isString: function (t) {
            return "string" == typeof t
        },
        isNumber: function (t) {
            return "number" == typeof t
        },
        isArray: t.isArray,
        isFunction: t.isFunction,
        isObject: t.isPlainObject,
        isUndefined: function (t) {
            return void 0 === t
        },
        bind: t.proxy,
        each: function (e, n) {
            t.each(e, function (t, e) {
                return n(e, t)
            })
        },
        map: t.map,
        filter: t.grep,
        every: function (e, n) {
            var i = !0;
            return e ? (t.each(e, function (t, r) {
                return !!(i = n.call(null, r, t, e)) && void 0
            }), !!i) : i
        },
        some: function (e, n) {
            var i = !1;
            return e ? (t.each(e, function (t, r) {
                return !(i = n.call(null, r, t, e)) && void 0
            }), !!i) : i
        },
        mixin: t.extend,
        getUniqueId: function () {
            var t = 0;
            return function () {
                return t++
            }
        }(),
        templatify: function (e) {
            return t.isFunction(e) ? e : function () {
                return String(e)
            }
        },
        defer: function (t) {
            setTimeout(t, 0)
        },
        debounce: function (t, e, n) {
            var i, r;
            return function () {
                var s, o, u = this,
                    a = arguments;
                return s = function () {
                    i = null, n || (r = t.apply(u, a))
                }, o = n && !i, clearTimeout(i), i = setTimeout(s, e), o && (r = t.apply(u, a)), r
            }
        },
        throttle: function (t, e) {
            var n, i, r, s, o, u;
            return o = 0, u = function () {
                o = new Date, r = null, s = t.apply(n, i)
            },
                function () {
                    var a = new Date,
                        h = e - (a - o);
                    return n = this, i = arguments, 0 >= h ? (clearTimeout(r), r = null, o = a, s = t.apply(n, i)) : r || (r = setTimeout(u, h)), s
                }
        },
        noop: function () { }
    },
        n = "0.10.2",
        i = function () {
            function t(t) {
                return t.split(/\s+/)
            }

            function e(t) {
                return t.split(/\W+/)
            }

            function n(t) {
                return function (e) {
                    return function (n) {
                        return t(n[e])
                    }
                }
            }
            return {
                nonword: e,
                whitespace: t,
                obj: {
                    nonword: n(e),
                    whitespace: n(t)
                }
            }
        }(),
        r = function () {
            function t(t) {
                this.maxSize = t || 100, this.size = 0, this.hash = {}, this.list = new n
            }

            function n() {
                this.head = this.tail = null
            }

            function i(t, e) {
                this.key = t, this.val = e, this.prev = this.next = null
            }
            return e.mixin(t.prototype, {
                set: function (t, e) {
                    var n, r = this.list.tail;
                    this.size >= this.maxSize && (this.list.remove(r), delete this.hash[r.key]), (n = this.hash[t]) ? (n.val = e, this.list.moveToFront(n)) : (n = new i(t, e), this.list.add(n), this.hash[t] = n, this.size++)
                },
                get: function (t) {
                    var e = this.hash[t];
                    return e ? (this.list.moveToFront(e), e.val) : void 0
                }
            }), e.mixin(n.prototype, {
                add: function (t) {
                    this.head && (t.next = this.head, this.head.prev = t), this.head = t, this.tail = this.tail || t
                },
                remove: function (t) {
                    t.prev ? t.prev.next = t.next : this.head = t.next, t.next ? t.next.prev = t.prev : this.tail = t.prev
                },
                moveToFront: function (t) {
                    this.remove(t), this.add(t)
                }
            }), t
        }(),
        s = function () {
            function t(t) {
                this.prefix = ["__", t, "__"].join(""), this.ttlKey = "__ttl__", this.keyMatcher = new RegExp("^" + this.prefix)
            }

            function n() {
                return (new Date).getTime()
            }

            function i(t) {
                return JSON.stringify(e.isUndefined(t) ? null : t)
            }

            function r(t) {
                return JSON.parse(t)
            }
            var s, o;
            try {
                (s = window.localStorage).setItem("~~~", "!"), s.removeItem("~~~")
            } catch (t) {
                s = null
            }
            return o = s && window.JSON ? {
                _prefix: function (t) {
                    return this.prefix + t
                },
                _ttlKey: function (t) {
                    return this._prefix(t) + this.ttlKey
                },
                get: function (t) {
                    return this.isExpired(t) && this.remove(t), r(s.getItem(this._prefix(t)))
                },
                set: function (t, r, o) {
                    return e.isNumber(o) ? s.setItem(this._ttlKey(t), i(n() + o)) : s.removeItem(this._ttlKey(t)), s.setItem(this._prefix(t), i(r))
                },
                remove: function (t) {
                    return s.removeItem(this._ttlKey(t)), s.removeItem(this._prefix(t)), this
                },
                clear: function () {
                    var t, e, n = [],
                        i = s.length;
                    for (t = 0; i > t; t++)(e = s.key(t)).match(this.keyMatcher) && n.push(e.replace(this.keyMatcher, ""));
                    for (t = n.length; t--;) this.remove(n[t]);
                    return this
                },
                isExpired: function (t) {
                    var i = r(s.getItem(this._ttlKey(t)));
                    return !!(e.isNumber(i) && n() > i)
                }
            } : {
                get: e.noop,
                set: e.noop,
                remove: e.noop,
                clear: e.noop,
                isExpired: e.noop
            }, e.mixin(t.prototype, o), t
        }(),
        o = function () {
            function n(e) {
                e = e || {}, this._send = e.transport ? i(e.transport) : t.ajax, this._get = e.rateLimiter ? e.rateLimiter(this._get) : this._get
            }

            function i(n) {
                return function (i, r) {
                    var s = t.Deferred();
                    return n(i, r, function (t) {
                        e.defer(function () {
                            s.resolve(t)
                        })
                    }, function (t) {
                        e.defer(function () {
                            s.reject(t)
                        })
                    }), s
                }
            }
            var s = 0,
                o = {},
                u = 6,
                a = new r(10);
            return n.setMaxPendingRequests = function (t) {
                u = t
            }, n.resetCache = function () {
                a = new r(10)
            }, e.mixin(n.prototype, {
                _get: function (t, e, n) {
                    function i(e) {
                        n && n(null, e), a.set(t, e)
                    }

                    function r() {
                        n && n(!0)
                    }
                    var h, c = this;
                    (h = o[t]) ? h.done(i).fail(r) : u > s ? (s++, o[t] = this._send(t, e).done(i).fail(r).always(function () {
                        s--, delete o[t], c.onDeckRequestArgs && (c._get.apply(c, c.onDeckRequestArgs), c.onDeckRequestArgs = null)
                    })) : this.onDeckRequestArgs = [].slice.call(arguments, 0)
                },
                get: function (t, n, i) {
                    var r;
                    return e.isFunction(n) && (i = n, n = {}), (r = a.get(t)) ? e.defer(function () {
                        i && i(null, r)
                    }) : this._get(t, n, i), !!r
                }
            }), n
        }(),
        u = function () {
            function n(e) {
                (e = e || {}).datumTokenizer && e.queryTokenizer || t.error("datumTokenizer and queryTokenizer are both required"), this.datumTokenizer = e.datumTokenizer, this.queryTokenizer = e.queryTokenizer, this.reset()
            }

            function i(t) {
                return t = e.filter(t, function (t) {
                    return !!t
                }), e.map(t, function (t) {
                    return t.toLowerCase()
                })
            }
            return e.mixin(n.prototype, {
                bootstrap: function (t) {
                    this.datums = t.datums, this.trie = t.trie
                },
                add: function (t) {
                    var n = this;
                    t = e.isArray(t) ? t : [t], e.each(t, function (t) {
                        var r, s;
                        r = n.datums.push(t) - 1, s = i(n.datumTokenizer(t)), e.each(s, function (t) {
                            var e, i, s;
                            for (e = n.trie, i = t.split(""); s = i.shift();)(e = e.children[s] || (e.children[s] = {
                                ids: [],
                                children: {}
                            })).ids.push(r)
                        })
                    })
                },
                get: function (t) {
                    var n, r, s = this;
                    return n = i(this.queryTokenizer(t)), e.each(n, function (t) {
                        var e, n, i, o;
                        if (r && 0 === r.length) return !1;
                        for (e = s.trie, n = t.split(""); e && (i = n.shift());) e = e.children[i];
                        return e && 0 === n.length ? (o = e.ids.slice(0), void (r = r ? function (t, e) {
                            function n(t, e) {
                                return t - e
                            }
                            var i = 0,
                                r = 0,
                                s = [];
                            for (t = t.sort(n), e = e.sort(n); i < t.length && r < e.length;) t[i] < e[r] ? i++ : t[i] > e[r] ? r++ : (s.push(t[i]), i++, r++);
                            return s
                        }(r, o) : o)) : (r = [], !1)
                    }), r ? e.map(function (t) {
                        for (var e = {}, n = [], i = 0; i < t.length; i++) e[t[i]] || (e[t[i]] = !0, n.push(t[i]));
                        return n
                    }(r), function (t) {
                        return s.datums[t]
                    }) : []
                },
                reset: function () {
                    this.datums = [], this.trie = {
                        ids: [],
                        children: {}
                    }
                },
                serialize: function () {
                    return {
                        datums: this.datums,
                        trie: this.trie
                    }
                }
            }), n
        }(),
        a = function () {
            return {
                local: function (t) {
                    return t.local || null
                },
                prefetch: function (i) {
                    var r, s;
                    return s = {
                        url: null,
                        thumbprint: "",
                        ttl: 864e5,
                        filter: null,
                        ajax: {}
                    }, (r = i.prefetch || null) && (r = e.isString(r) ? {
                        url: r
                    } : r, (r = e.mixin(s, r)).thumbprint = n + r.thumbprint, r.ajax.type = r.ajax.type || "GET", r.ajax.dataType = r.ajax.dataType || "json", !r.url && t.error("prefetch requires url to be set")), r
                },
                remote: function (n) {
                    var i, r;
                    return r = {
                        url: null,
                        wildcard: "%QUERY",
                        replace: null,
                        rateLimitBy: "debounce",
                        rateLimitWait: 300,
                        send: null,
                        filter: null,
                        ajax: {}
                    }, (i = n.remote || null) && (i = e.isString(i) ? {
                        url: i
                    } : i, (i = e.mixin(r, i)).rateLimiter = /^throttle$/i.test(i.rateLimitBy) ? function (t) {
                        return function (n) {
                            return e.throttle(n, t)
                        }
                    }(i.rateLimitWait) : function (t) {
                        return function (n) {
                            return e.debounce(n, t)
                        }
                    }(i.rateLimitWait), i.ajax.type = i.ajax.type || "GET", i.ajax.dataType = i.ajax.dataType || "json", delete i.rateLimitBy, delete i.rateLimitWait, !i.url && t.error("remote requires url to be set")), i
                }
            }
        }();
    ! function (n) {
        function r(e) {
            e && (e.local || e.prefetch || e.remote) || t.error("one of local, prefetch, or remote is required"), this.limit = e.limit || 5, this.sorter = h(e.sorter), this.dupDetector = e.dupDetector || c, this.local = a.local(e), this.prefetch = a.prefetch(e), this.remote = a.remote(e), this.cacheKey = this.prefetch ? this.prefetch.cacheKey || this.prefetch.url : null, this.index = new u({
                datumTokenizer: e.datumTokenizer,
                queryTokenizer: e.queryTokenizer
            }), this.storage = this.cacheKey ? new s(this.cacheKey) : null
        }

        function h(t) {
            return e.isFunction(t) ? function (e) {
                return e.sort(t)
            } : function (t) {
                return t
            }
        }

        function c() {
            return !1
        }
        var l, d;
        l = n.Bloodhound, d = {
            data: "data",
            protocol: "protocol",
            thumbprint: "thumbprint"
        }, n.Bloodhound = r, r.noConflict = function () {
            return n.Bloodhound = l, r
        }, r.tokenizers = i, e.mixin(r.prototype, {
            _loadPrefetch: function (e) {
                var n, i, r = this;
                return (n = this._readFromStorage(e.thumbprint)) ? (this.index.bootstrap(n), i = t.Deferred().resolve()) : i = t.ajax(e.url, e.ajax).done(function (t) {
                    r.clear(), r.add(e.filter ? e.filter(t) : t), r._saveToStorage(r.index.serialize(), e.thumbprint, e.ttl)
                }), i
            },
            _getFromRemote: function (t, e) {
                var n, i, r = this;
                return t = t || "", i = encodeURIComponent(t), n = this.remote.replace ? this.remote.replace(this.remote.url, t) : this.remote.url.replace(this.remote.wildcard, i), this.transport.get(n, this.remote.ajax, function (t, n) {
                    e(t ? [] : r.remote.filter ? r.remote.filter(n) : n)
                })
            },
            _saveToStorage: function (t, e, n) {
                this.storage && (this.storage.set(d.data, t, n), this.storage.set(d.protocol, location.protocol, n), this.storage.set(d.thumbprint, e, n))
            },
            _readFromStorage: function (t) {
                var e, n = {};
                return this.storage && (n.data = this.storage.get(d.data), n.protocol = this.storage.get(d.protocol), n.thumbprint = this.storage.get(d.thumbprint)), e = n.thumbprint !== t || n.protocol !== location.protocol, n.data && !e ? n.data : null
            },
            _initialize: function () {
                var n, i = this,
                    r = this.local;
                return n = this.prefetch ? this._loadPrefetch(this.prefetch) : t.Deferred().resolve(), r && n.done(function () {
                    i.add(e.isFunction(r) ? r() : r)
                }), this.transport = this.remote ? new o(this.remote) : null, this.initPromise = n.promise()
            },
            initialize: function (t) {
                return !this.initPromise || t ? this._initialize() : this.initPromise
            },
            add: function (t) {
                this.index.add(t)
            },
            get: function (t, n) {
                var i = this,
                    r = [],
                    s = !1;
                r = this.index.get(t), (r = this.sorter(r).slice(0, this.limit)).length < this.limit && this.transport && (s = this._getFromRemote(t, function (t) {
                    var s = r.slice(0);
                    e.each(t, function (t) {
                        return !e.some(s, function (e) {
                            return i.dupDetector(t, e)
                        }) && s.push(t), s.length < i.limit
                    }), n && n(i.sorter(s))
                })), s || (r.length > 0 || !this.transport) && n && n(r)
            },
            clear: function () {
                this.index.reset()
            },
            clearPrefetchCache: function () {
                this.storage && this.storage.clear()
            },
            clearRemoteCache: function () {
                this.transport && o.resetCache()
            },
            ttAdapter: function () {
                return e.bind(this.get, this)
            }
        })
    }(this);
    var h = {
        wrapper: '<span class="twitter-typeahead"></span>',
        dropdown: '<span class="tt-dropdown-menu"></span>',
        dataset: '<div class="tt-dataset-%CLASS%"></div>',
        suggestions: '<span class="tt-suggestions"></span>',
        suggestion: '<div class="tt-suggestion"></div>'
    },
        c = {
            wrapper: {
                position: "relative",
                display: "block"
            },
            hint: {
                position: "absolute",
                top: "0",
                left: "0",
                borderColor: "transparent",
                boxShadow: "none"
            },
            input: {
                position: "relative",
                verticalAlign: "top",
                backgroundColor: "transparent"
            },
            inputWithNoHint: {
                position: "relative",
                verticalAlign: "top"
            },
            dropdown: {
                position: "absolute",
                top: "100%",
                left: "0",
                zIndex: "100",
                display: "none"
            },
            suggestions: {
                display: "block"
            },
            suggestion: {
                whiteSpace: "nowrap",
                cursor: "pointer"
            },
            suggestionChild: {
                whiteSpace: "normal"
            },
            ltr: {
                left: "0",
                right: "auto"
            },
            rtl: {
                left: "auto",
                right: " 0"
            }
        };
    e.isMsie() && e.mixin(c.input, {
        backgroundImage: "url(data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7)"
    }), e.isMsie() && e.isMsie() <= 7 && e.mixin(c.input, {
        marginTop: "-1px"
    });
    var l = function () {
        function n(e) {
            e && e.el || t.error("EventBus initialized without el"), this.$el = t(e.el)
        }
        return e.mixin(n.prototype, {
            trigger: function (t) {
                var e = [].slice.call(arguments, 1);
                this.$el.trigger("typeahead:" + t, e)
            }
        }), n
    }(),
        d = function () {
            function t(t, e, i, r) {
                var s;
                if (!i) return this;
                for (e = e.split(n), i = r ? function (t, e) {
                    return t.bind ? t.bind(e) : function () {
                        t.apply(e, [].slice.call(arguments, 0))
                    }
                }(i, r) : i, this._callbacks = this._callbacks || {}; s = e.shift();) this._callbacks[s] = this._callbacks[s] || {
                    sync: [],
                    async: []
                }, this._callbacks[s][t].push(i);
                return this
            }

            function e(t, e, n) {
                return function () {
                    for (var i, r = 0; !i && r < t.length; r += 1) i = !1 === t[r].apply(e, n);
                    return !i
                }
            }
            var n = /\s+/,
                i = window.setImmediate ? function (t) {
                    setImmediate(function () {
                        t()
                    })
                } : function (t) {
                    setTimeout(function () {
                        t()
                    }, 0)
                };
            return {
                onSync: function (e, n, i) {
                    return t.call(this, "sync", e, n, i)
                },
                onAsync: function (e, n, i) {
                    return t.call(this, "async", e, n, i)
                },
                off: function (t) {
                    var e;
                    if (!this._callbacks) return this;
                    for (t = t.split(n); e = t.shift();) delete this._callbacks[e];
                    return this
                },
                trigger: function (t) {
                    var r, s, o, u, a;
                    if (!this._callbacks) return this;
                    for (t = t.split(n), o = [].slice.call(arguments, 1);
                        (r = t.shift()) && (s = this._callbacks[r]);) u = e(s.sync, this, [r].concat(o)), a = e(s.async, this, [r].concat(o)), u() && i(a);
                    return this
                }
            }
        }(),
        p = function (t) {
            var n = {
                node: null,
                pattern: null,
                tagName: "strong",
                className: null,
                wordsOnly: !1,
                caseSensitive: !1
            };
            return function (i) {
                var r;
                (i = e.mixin({}, n, i)).node && i.pattern && (i.pattern = e.isArray(i.pattern) ? i.pattern : [i.pattern], r = function (t, n, i) {
                    for (var r, s = [], o = 0; o < t.length; o++) s.push(e.escapeRegExChars(t[o]));
                    return r = i ? "\\b(" + s.join("|") + ")\\b" : "(" + s.join("|") + ")", n ? new RegExp(r) : new RegExp(r, "i")
                }(i.pattern, i.caseSensitive, i.wordsOnly), function t(e, n) {
                    for (var i, r = 0; r < e.childNodes.length; r++) 3 === (i = e.childNodes[r]).nodeType ? r += n(i) ? 1 : 0 : t(i, n)
                }(i.node, function (e) {
                    var n, s;
                    return (n = r.exec(e.data)) && (wrapperNode = t.createElement(i.tagName), i.className && (wrapperNode.className = i.className), (s = e.splitText(n.index)).splitText(n[0].length), wrapperNode.appendChild(s.cloneNode(!0)), e.parentNode.replaceChild(wrapperNode, s)), !!n
                }))
            }
        }(window.document),
        f = function () {
            function n(n) {
                var i, r, o, u, a = this;
                (n = n || {}).input || t.error("input is missing"), i = e.bind(this._onBlur, this), r = e.bind(this._onFocus, this), o = e.bind(this._onKeydown, this), u = e.bind(this._onInput, this), this.$hint = t(n.hint), this.$input = t(n.input).on("blur.tt", i).on("focus.tt", r).on("keydown.tt", o), 0 === this.$hint.length && (this.setHint = this.getHint = this.clearHint = this.clearHintIfInvalid = e.noop), e.isMsie() ? this.$input.on("keydown.tt keypress.tt cut.tt paste.tt", function (t) {
                    s[t.which || t.keyCode] || e.defer(e.bind(a._onInput, a, t))
                }) : this.$input.on("input.tt", u), this.query = this.$input.val(), this.$overflowHelper = function (e) {
                    return t('<pre aria-hidden="true"></pre>').css({
                        position: "absolute",
                        visibility: "hidden",
                        whiteSpace: "pre",
                        fontFamily: e.css("font-family"),
                        fontSize: e.css("font-size"),
                        fontStyle: e.css("font-style"),
                        fontVariant: e.css("font-variant"),
                        fontWeight: e.css("font-weight"),
                        wordSpacing: e.css("word-spacing"),
                        letterSpacing: e.css("letter-spacing"),
                        textIndent: e.css("text-indent"),
                        textRendering: e.css("text-rendering"),
                        textTransform: e.css("text-transform")
                    }).insertAfter(e)
                }(this.$input)
            }

            function i(t, e) {
                return n.normalizeQuery(t) === n.normalizeQuery(e)
            }

            function r(t) {
                return t.altKey || t.ctrlKey || t.metaKey || t.shiftKey
            }
            var s;
            return s = {
                9: "tab",
                27: "esc",
                37: "left",
                39: "right",
                13: "enter",
                38: "up",
                40: "down"
            }, n.normalizeQuery = function (t) {
                return (t || "").replace(/^\s*/g, "").replace(/\s{2,}/g, " ")
            }, e.mixin(n.prototype, d, {
                _onBlur: function () {
                    this.resetInputValue(), this.trigger("blurred")
                },
                _onFocus: function () {
                    this.trigger("focused")
                },
                _onKeydown: function (t) {
                    var e = s[t.which || t.keyCode];
                    this._managePreventDefault(e, t), e && this._shouldTrigger(e, t) && this.trigger(e + "Keyed", t)
                },
                _onInput: function () {
                    this._checkInputValue()
                },
                _managePreventDefault: function (t, e) {
                    var n, i, s;
                    switch (t) {
                        case "tab":
                            i = this.getHint(), s = this.getInputValue(), n = i && i !== s && !r(e);
                            break;
                        case "up":
                        case "down":
                            n = !r(e);
                            break;
                        default:
                            n = !1
                    }
                    n && e.preventDefault()
                },
                _shouldTrigger: function (t, e) {
                    var n;
                    switch (t) {
                        case "tab":
                            n = !r(e);
                            break;
                        default:
                            n = !0
                    }
                    return n
                },
                _checkInputValue: function () {
                    var t, e, n;
                    n = !!(e = i(t = this.getInputValue(), this.query)) && this.query.length !== t.length, e ? n && this.trigger("whitespaceChanged", this.query) : this.trigger("queryChanged", this.query = t)
                },
                focus: function () {
                    this.$input.focus()
                },
                blur: function () {
                    this.$input.blur()
                },
                getQuery: function () {
                    return this.query
                },
                setQuery: function (t) {
                    this.query = t
                },
                getInputValue: function () {
                    return this.$input.val()
                },
                setInputValue: function (t, e) {
                    this.$input.val(t), e ? this.clearHint() : this._checkInputValue()
                },
                resetInputValue: function () {
                    this.setInputValue(this.query, !0)
                },
                getHint: function () {
                    return this.$hint.val()
                },
                setHint: function (t) {
                    this.$hint.val(t)
                },
                clearHint: function () {
                    this.setHint("")
                },
                clearHintIfInvalid: function () {
                    var t, e, n;
                    n = (t = this.getInputValue()) !== (e = this.getHint()) && 0 === e.indexOf(t), !("" !== t && n && !this.hasOverflow()) && this.clearHint()
                },
                getLanguageDirection: function () {
                    return (this.$input.css("direction") || "ltr").toLowerCase()
                },
                hasOverflow: function () {
                    var t = this.$input.width() - 2;
                    return this.$overflowHelper.text(this.getInputValue()), this.$overflowHelper.width() >= t
                },
                isCursorAtEnd: function () {
                    var t, n, i;
                    return t = this.$input.val().length, n = this.$input[0].selectionStart, e.isNumber(n) ? n === t : !document.selection || ((i = document.selection.createRange()).moveStart("character", -t), t === i.text.length)
                },
                destroy: function () {
                    this.$hint.off(".tt"), this.$input.off(".tt"), this.$hint = this.$input = this.$overflowHelper = null
                }
            }), n
        }(),
        g = function () {
            function n(n) {
                (n = n || {}).templates = n.templates || {}, n.source || t.error("missing source"), n.name && ! function (t) {
                    return /^[_a-zA-Z0-9-]+$/.test(t)
                }(n.name) && t.error("invalid dataset name: " + n.name), this.query = null, this.highlight = !!n.highlight, this.name = n.name || e.getUniqueId(), this.source = n.source, this.displayFn = function (t) {
                    return t = t || "value", e.isFunction(t) ? t : function (e) {
                        return e[t]
                    }
                }(n.display || n.displayKey), this.templates = function (t, n) {
                    return {
                        empty: t.empty && e.templatify(t.empty),
                        header: t.header && e.templatify(t.header),
                        footer: t.footer && e.templatify(t.footer),
                        suggestion: t.suggestion || function (t) {
                            return "<p>" + n(t) + "</p>"
                        }
                    }
                }(n.templates, this.displayFn), this.$el = t(h.dataset.replace("%CLASS%", this.name))
            }
            var i = "ttDataset",
                r = "ttValue",
                s = "ttDatum";
            return n.extractDatasetName = function (e) {
                return t(e).data(i)
            }, n.extractValue = function (e) {
                return t(e).data(r)
            }, n.extractDatum = function (e) {
                return t(e).data(s)
            }, e.mixin(n.prototype, d, {
                _render: function (n, o) {
                    function u() {
                        return d.templates.header({
                            query: n,
                            isEmpty: !l
                        })
                    }

                    function a() {
                        return d.templates.footer({
                            query: n,
                            isEmpty: !l
                        })
                    }
                    if (this.$el) {
                        var l, d = this;
                        this.$el.empty(), !(l = o && o.length) && this.templates.empty ? this.$el.html(d.templates.empty({
                            query: n,
                            isEmpty: !0
                        })).prepend(d.templates.header ? u() : null).append(d.templates.footer ? a() : null) : l && this.$el.html(function () {
                            var u, a;
                            return u = t(h.suggestions).css(c.suggestions), a = e.map(o, function (e) {
                                var n;
                                return (n = t(h.suggestion).append(d.templates.suggestion(e)).data(i, d.name).data(r, d.displayFn(e)).data(s, e)).children().each(function () {
                                    t(this).css(c.suggestionChild)
                                }), n
                            }), u.append.apply(u, a), d.highlight && p({
                                node: u[0],
                                pattern: n
                            }), u
                        }()).prepend(d.templates.header ? u() : null).append(d.templates.footer ? a() : null), this.trigger("rendered")
                    }
                },
                getRoot: function () {
                    return this.$el
                },
                update: function (t) {
                    var e = this;
                    this.query = t, this.canceled = !1, this.source(t, function (n) {
                        e.canceled || t !== e.query || e._render(t, n)
                    })
                },
                cancel: function () {
                    this.canceled = !0
                },
                clear: function () {
                    this.cancel(), this.$el.empty(), this.trigger("rendered")
                },
                isEmpty: function () {
                    return this.$el.is(":empty")
                },
                destroy: function () {
                    this.$el = null
                }
            }), n
        }(),
        m = function () {
            function n(n) {
                var r, s, o, u = this;
                (n = n || {}).menu || t.error("menu is required"), this.isOpen = !1, this.isEmpty = !0, this.datasets = e.map(n.datasets, i), r = e.bind(this._onSuggestionClick, this), s = e.bind(this._onSuggestionMouseEnter, this), o = e.bind(this._onSuggestionMouseLeave, this), this.$menu = t(n.menu).on("click.tt", ".tt-suggestion", r).on("mouseenter.tt", ".tt-suggestion", s).on("mouseleave.tt", ".tt-suggestion", o), e.each(this.datasets, function (t) {
                    u.$menu.append(t.getRoot()), t.onSync("rendered", u._onRendered, u)
                })
            }

            function i(t) {
                return new g(t)
            }
            return e.mixin(n.prototype, d, {
                _onSuggestionClick: function (e) {
                    this.trigger("suggestionClicked", t(e.currentTarget))
                },
                _onSuggestionMouseEnter: function (e) {
                    this._removeCursor(), this._setCursor(t(e.currentTarget), !0)
                },
                _onSuggestionMouseLeave: function () {
                    this._removeCursor()
                },
                _onRendered: function () {
                    this.isEmpty = e.every(this.datasets, function (t) {
                        return t.isEmpty()
                    }), this.isEmpty ? this._hide() : this.isOpen && this._show(), this.trigger("datasetRendered")
                },
                _hide: function () {
                    this.$menu.hide()
                },
                _show: function () {
                    this.$menu.css("display", "block")
                },
                _getSuggestions: function () {
                    return this.$menu.find(".tt-suggestion")
                },
                _getCursor: function () {
                    return this.$menu.find(".tt-cursor").first()
                },
                _setCursor: function (t, e) {
                    t.first().addClass("tt-cursor"), !e && this.trigger("cursorMoved")
                },
                _removeCursor: function () {
                    this._getCursor().removeClass("tt-cursor")
                },
                _moveCursor: function (t) {
                    var e, n, i, r;
                    if (this.isOpen) {
                        if (n = this._getCursor(), e = this._getSuggestions(), this._removeCursor(), -1 === (i = ((i = e.index(n) + t) + 1) % (e.length + 1) - 1)) return void this.trigger("cursorRemoved"); - 1 > i && (i = e.length - 1), this._setCursor(r = e.eq(i)), this._ensureVisible(r)
                    }
                },
                _ensureVisible: function (t) {
                    var e, n, i, r;
                    n = (e = t.position().top) + t.outerHeight(!0), i = this.$menu.scrollTop(), r = this.$menu.height() + parseInt(this.$menu.css("paddingTop"), 10) + parseInt(this.$menu.css("paddingBottom"), 10), 0 > e ? this.$menu.scrollTop(i + e) : n > r && this.$menu.scrollTop(i + (n - r))
                },
                close: function () {
                    this.isOpen && (this.isOpen = !1, this._removeCursor(), this._hide(), this.trigger("closed"))
                },
                open: function () {
                    this.isOpen || (this.isOpen = !0, !this.isEmpty && this._show(), this.trigger("opened"))
                },
                setLanguageDirection: function (t) {
                    this.$menu.css("ltr" === t ? c.ltr : c.rtl)
                },
                moveCursorUp: function () {
                    this._moveCursor(-1)
                },
                moveCursorDown: function () {
                    this._moveCursor(1)
                },
                getDatumForSuggestion: function (t) {
                    var e = null;
                    return t.length && (e = {
                        raw: g.extractDatum(t),
                        value: g.extractValue(t),
                        datasetName: g.extractDatasetName(t)
                    }), e
                },
                getDatumForCursor: function () {
                    return this.getDatumForSuggestion(this._getCursor().first())
                },
                getDatumForTopSuggestion: function () {
                    return this.getDatumForSuggestion(this._getSuggestions().first())
                },
                update: function (t) {
                    e.each(this.datasets, function (e) {
                        e.update(t)
                    })
                },
                empty: function () {
                    e.each(this.datasets, function (t) {
                        t.clear()
                    }), this.isEmpty = !0
                },
                isVisible: function () {
                    return this.isOpen && !this.isEmpty
                },
                destroy: function () {
                    this.$menu.off(".tt"), this.$menu = null, e.each(this.datasets, function (t) {
                        t.destroy()
                    })
                }
            }), n
        }(),
        y = function () {
            function n(n) {
                var r, s, o;
                (n = n || {}).input || t.error("missing input"), this.isActivated = !1, this.autoselect = !!n.autoselect, this.minLength = e.isNumber(n.minLength) ? n.minLength : 1, this.$node = i(n.input, n.withHint), r = this.$node.find(".tt-dropdown-menu"), s = this.$node.find(".tt-input"), o = this.$node.find(".tt-hint"), s.on("blur.tt", function (t) {
                    var n, i, o;
                    n = document.activeElement, i = r.is(n), o = r.has(n).length > 0, e.isMsie() && (i || o) && (t.preventDefault(), t.stopImmediatePropagation(), e.defer(function () {
                        s.focus()
                    }))
                }), r.on("mousedown.tt", function (t) {
                    t.preventDefault()
                }), this.eventBus = n.eventBus || new l({
                    el: s
                }), this.dropdown = new m({
                    menu: r,
                    datasets: n.datasets
                }).onSync("suggestionClicked", this._onSuggestionClicked, this).onSync("cursorMoved", this._onCursorMoved, this).onSync("cursorRemoved", this._onCursorRemoved, this).onSync("opened", this._onOpened, this).onSync("closed", this._onClosed, this).onAsync("datasetRendered", this._onDatasetRendered, this), this.input = new f({
                    input: s,
                    hint: o
                }).onSync("focused", this._onFocused, this).onSync("blurred", this._onBlurred, this).onSync("enterKeyed", this._onEnterKeyed, this).onSync("tabKeyed", this._onTabKeyed, this).onSync("escKeyed", this._onEscKeyed, this).onSync("upKeyed", this._onUpKeyed, this).onSync("downKeyed", this._onDownKeyed, this).onSync("leftKeyed", this._onLeftKeyed, this).onSync("rightKeyed", this._onRightKeyed, this).onSync("queryChanged", this._onQueryChanged, this).onSync("whitespaceChanged", this._onWhitespaceChanged, this), this._setLanguageDirection()
            }

            function i(e, n) {
                var i, s, o, u;
                i = t(e), s = t(h.wrapper).css(c.wrapper), o = t(h.dropdown).css(c.dropdown), (u = i.clone().css(c.hint).css(function (t) {
                    return {
                        backgroundAttachment: t.css("background-attachment"),
                        backgroundClip: t.css("background-clip"),
                        backgroundColor: t.css("background-color"),
                        backgroundImage: t.css("background-image"),
                        backgroundOrigin: t.css("background-origin"),
                        backgroundPosition: t.css("background-position"),
                        backgroundRepeat: t.css("background-repeat"),
                        backgroundSize: t.css("background-size")
                    }
                }(i))).val("").removeData().addClass("tt-hint").removeAttr("id name placeholder").prop("disabled", !0).attr({
                    autocomplete: "off",
                    spellcheck: "false"
                }), i.data(r, {
                    dir: i.attr("dir"),
                    autocomplete: i.attr("autocomplete"),
                    spellcheck: i.attr("spellcheck"),
                    style: i.attr("style")
                }), i.addClass("tt-input").attr({
                    autocomplete: "off",
                    spellcheck: !1
                }).css(n ? c.input : c.inputWithNoHint);
                try {
                    !i.attr("dir") && i.attr("dir", "auto")
                } catch (t) { }
                return i.wrap(s).parent().prepend(n ? u : null).append(o)
            }
            var r = "ttAttrs";
            return e.mixin(n.prototype, {
                _onSuggestionClicked: function (t, e) {
                    var n;
                    (n = this.dropdown.getDatumForSuggestion(e)) && this._select(n)
                },
                _onCursorMoved: function () {
                    var t = this.dropdown.getDatumForCursor();
                    this.input.setInputValue(t.value, !0), this.eventBus.trigger("cursorchanged", t.raw, t.datasetName)
                },
                _onCursorRemoved: function () {
                    this.input.resetInputValue(), this._updateHint()
                },
                _onDatasetRendered: function () {
                    this._updateHint()
                },
                _onOpened: function () {
                    this._updateHint(), this.eventBus.trigger("opened")
                },
                _onClosed: function () {
                    this.input.clearHint(), this.eventBus.trigger("closed")
                },
                _onFocused: function () {
                    this.isActivated = !0, this.dropdown.open()
                },
                _onBlurred: function () {
                    this.isActivated = !1, this.dropdown.empty(), this.dropdown.close()
                },
                _onEnterKeyed: function (t, e) {
                    var n, i;
                    n = this.dropdown.getDatumForCursor(), i = this.dropdown.getDatumForTopSuggestion(), n ? (this._select(n), e.preventDefault()) : this.autoselect && i && (this._select(i), e.preventDefault())
                },
                _onTabKeyed: function (t, e) {
                    var n;
                    (n = this.dropdown.getDatumForCursor()) ? (this._select(n), e.preventDefault()) : this._autocomplete(!0)
                },
                _onEscKeyed: function () {
                    this.dropdown.close(), this.input.resetInputValue()
                },
                _onUpKeyed: function () {
                    var t = this.input.getQuery();
                    this.dropdown.isEmpty && t.length >= this.minLength ? this.dropdown.update(t) : this.dropdown.moveCursorUp(), this.dropdown.open()
                },
                _onDownKeyed: function () {
                    var t = this.input.getQuery();
                    this.dropdown.isEmpty && t.length >= this.minLength ? this.dropdown.update(t) : this.dropdown.moveCursorDown(), this.dropdown.open()
                },
                _onLeftKeyed: function () {
                    "rtl" === this.dir && this._autocomplete()
                },
                _onRightKeyed: function () {
                    "ltr" === this.dir && this._autocomplete()
                },
                _onQueryChanged: function (t, e) {
                    this.input.clearHintIfInvalid(), e.length >= this.minLength ? this.dropdown.update(e) : this.dropdown.empty(), this.dropdown.open(), this._setLanguageDirection()
                },
                _onWhitespaceChanged: function () {
                    this._updateHint(), this.dropdown.open()
                },
                _setLanguageDirection: function () {
                    var t;
                    this.dir !== (t = this.input.getLanguageDirection()) && (this.dir = t, this.$node.css("direction", t), this.dropdown.setLanguageDirection(t))
                },
                _updateHint: function () {
                    var t, n, i, r, s;
                    (t = this.dropdown.getDatumForTopSuggestion()) && this.dropdown.isVisible() && !this.input.hasOverflow() ? (n = this.input.getInputValue(), i = f.normalizeQuery(n), r = e.escapeRegExChars(i), (s = new RegExp("^(?:" + r + ")(.+$)", "i").exec(t.value)) ? this.input.setHint(n + s[1]) : this.input.clearHint()) : this.input.clearHint()
                },
                _autocomplete: function (t) {
                    var e, n, i, r;
                    e = this.input.getHint(), n = this.input.getQuery(), i = t || this.input.isCursorAtEnd(), e && n !== e && i && ((r = this.dropdown.getDatumForTopSuggestion()) && this.input.setInputValue(r.value), this.eventBus.trigger("autocompleted", r.raw, r.datasetName))
                },
                _select: function (t) {
                    this.input.setQuery(t.value), this.input.setInputValue(t.value, !0), this._setLanguageDirection(), this.eventBus.trigger("selected", t.raw, t.datasetName), this.dropdown.close(), e.defer(e.bind(this.dropdown.empty, this.dropdown))
                },
                open: function () {
                    this.dropdown.open()
                },
                close: function () {
                    this.dropdown.close()
                },
                setVal: function (t) {
                    this.isActivated ? this.input.setInputValue(t) : (this.input.setQuery(t), this.input.setInputValue(t, !0)), this._setLanguageDirection()
                },
                getVal: function () {
                    return this.input.getQuery()
                },
                destroy: function () {
                    var t, n;
                    this.input.destroy(), this.dropdown.destroy(), t = this.$node, n = t.find(".tt-input"), e.each(n.data(r), function (t, i) {
                        e.isUndefined(t) ? n.removeAttr(i) : n.attr(i, t)
                    }), n.detach().removeData(r).removeClass("tt-input").insertAfter(t), t.remove(), this.$node = null
                }
            }), n
        }();
    ! function () {
        var n, i, r;
        n = t.fn.typeahead, i = "ttTypeahead", r = {
            initialize: function (n, r) {
                return r = e.isArray(r) ? r : [].slice.call(arguments, 1), n = n || {}, this.each(function () {
                    var s, o = t(this);
                    e.each(r, function (t) {
                        t.highlight = !!n.highlight
                    }), s = new y({
                        input: o,
                        eventBus: new l({
                            el: o
                        }),
                        withHint: !!e.isUndefined(n.hint) || !!n.hint,
                        minLength: n.minLength,
                        autoselect: n.autoselect,
                        datasets: r
                    }), o.data(i, s)
                })
            },
            open: function () {
                return this.each(function () {
                    var e;
                    (e = t(this).data(i)) && e.open()
                })
            },
            close: function () {
                return this.each(function () {
                    var e;
                    (e = t(this).data(i)) && e.close()
                })
            },
            val: function (e) {
                return arguments.length ? this.each(function () {
                    var n;
                    (n = t(this).data(i)) && n.setVal(e)
                }) : function (t) {
                    var e, n;
                    return (e = t.data(i)) && (n = e.getVal()), n
                }(this.first())
            },
            destroy: function () {
                return this.each(function () {
                    var e, n = t(this);
                    (e = n.data(i)) && (e.destroy(), n.removeData(i))
                })
            }
        }, t.fn.typeahead = function (t) {
            return r[t] ? r[t].apply(this, [].slice.call(arguments, 1)) : r.initialize.apply(this, arguments)
        }, t.fn.typeahead.noConflict = function () {
            return t.fn.typeahead = n, this
        }
    }()
}(window.jQuery);