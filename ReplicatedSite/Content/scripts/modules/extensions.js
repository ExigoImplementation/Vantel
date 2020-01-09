define(function() {
// Check for CultureInfo - warn the console if we can't find it.
    if (typeof CultureInfo === "undefined") console.warn("Culture formatting not enabled (CultureInfo missing)");


// StringBuilder module
    StringBuilder = function(a) {
        this._parts = typeof a !== "undefined" && a !== null && a !== "" ? [a.toString()] : [];
        this._value = {};
        this._len = 0
    };
    StringBuilder.prototype = {
        append: function(a) {
            this._parts[this._parts.length] = a
        },
        appendLine: function(a) {
            this._parts[this._parts.length] = typeof a === "undefined" || a === null || a === "" ? "\r\n" : a + "\r\n"
        },
        clear: function() {
            this._parts = [];
            this._value = {};
            this._len = 0
        },
        isEmpty: function() {
            if (this._parts.length === 0) return true;
            return this.toString() === ""
        },
        toString: function(a) {
            a = a || "";
            var b = this._parts;
            if (this._len !== b.length) {
                this._value = {};
                this._len = b.length
            }
            var d = this._value;
            if (typeof d[a] === "undefined") {
                if (a !== "")
                    for (var c = 0; c < b.length;)
                        if (typeof b[c] === "undefined" || b[c] === "" || b[c] === null) b.splice(c, 1);
                        else c++;
                d[a] = this._parts.join(a)
            }
            return d[a]
        }
    };


// Array extensions
    if (typeof Array.prototype.indexOf !== 'function') {
        Array.prototype.indexOf = function(item, i) {
            if (this == null) throw new TypeError();

            var array = Object(this), length = array.length >>> 0;
            if (length === 0) return -1;

            i = Number(i);
            if (isNaN(i)) {
                i = 0;
            } else if (i !== 0 && isFinite(i)) {
                i = (i > 0 ? 1 : -1) * Math.floor(Math.abs(i));
            }

            if (i > length) return -1;

            var k = i >= 0 ? i : Math.max(length - Math.abs(i), 0);
            for (; k < length; k++)
                if (k in array && array[k] === item) return k;
            return -1;
        }
    }
    if (typeof Array.prototype.add !== 'function') {
        Array.prototype.add = function(item) {
            this[this.length] = item;
        }
    }
    if (typeof Array.prototype.addRange !== 'function') {
        Array.prototype.addRange = function(array) {
            this.push.apply(this, array);
        }
    }
    if (typeof Array.prototype.clear !== 'function') {
        Array.prototype.clear = function() {
            this.length = 0;
        }
    }
    if (typeof Array.prototype.contains !== 'function') {
        Array.prototype.contains = function(item) {
            if (typeof item === "undefined") return false;
            var c = this.length;
            if (c !== 0) {
                item = item - 0;
                if (isNaN(item)) item = 0;
                else {
                    if (isFinite(item)) item = item - item % 1;
                    if (item < 0) item = Math.max(0, c + item)
                }
                for (var b = item; b < c; b++)
                    if (typeof this[b] !== "undefined" && this[b] === item) return true;
            }
            return false;
        }
    }
    if (typeof Array.prototype.forEach !== 'function') {
        Array.prototype.forEach = function(array, method, context) {
            for (var a = 0, max = array.length; a < max; a++) {
                var c = array[a];
                if (typeof c !== "undefined") method.call(context, c, a, array);
            }
        }
    }
    if (typeof Array.prototype.remove !== 'function') {
        Array.prototype.remove = function(array, item) {
            var a = Sys._indexOf(array, item);
            if (a >= 0) array.splice(a, 1);
            return a >= 0;
        }
    }


// Date extensions
    if (typeof Date.prototype.format !== "function" && typeof CultureInfo !== "undefined") {
        Date.prototype.format = function(a) {
            return this._toFormattedString(a, CultureInfo.InvariantCulture)
        };
        Date.prototype.localeFormat = function(a) {
            return this._toFormattedString(a, CultureInfo.CurrentCulture)
        };
        Date.prototype._toFormattedString = function(e, j) {
            var b = j.dateTimeFormat,
                n = b.Calendar.convert;
            if (!e || !e.length || e === "i")
                if (j && j.name.length)
                    if (n) return this._toFormattedString(b.FullDateTimePattern, j);
                    else {
                        var r = new Date(this.getTime()),
                            x = Date._getEra(this, b.eras);
                        r.setFullYear(Date._getEraYear(this, b, x));
                        return r.toLocaleString()
                    }
                else return this.toString();
            var l = b.eras,
                k = e === "s";
            e = Date._expandFormat(b, e);
            var a = new StringBuilder,
                c;

            function d(a) {
                if (a < 10) return "0" + a;
                return a.toString()
            }

            function m(a) {
                if (a < 10) return "00" + a;
                if (a < 100) return "0" + a;
                return a.toString()
            }

            function v(a) {
                if (a < 10) return "000" + a;
                else if (a < 100) return "00" + a;
                else if (a < 1000) return "0" + a;
                return a.toString()
            }

            var h, p, t = /([^d]|^)(d|dd)([^d]|$)/g;

            function s() {
                if (h || p) return h;
                h = t.test(e);
                p = true;
                return h
            }

            var q = 0,
                o = Date._getTokenRegExp(),
                f;
            if (!k && n) f = n.fromGregorian(this);
            for (; true;) {
                var w = o.lastIndex,
                    i = o.exec(e),
                    u = e.slice(w, i ? i.index : e.length);
                q += Date._appendPreOrPostMatch(u, a);
                if (!i) break;
                if (q % 2 === 1) {
                    a.append(i[0]);
                    continue
                }

                function g(a, b) {
                    if (f) return f[b];
                    switch (b) {
                    case 0:
                        return a.getFullYear();
                    case 1:
                        return a.getMonth();
                    case 2:
                        return a.getDate()
                    }
                }

                switch (i[0]) {
                case "dddd":
                    a.append(b.DayNames[this.getDay()]);
                    break;
                case "ddd":
                    a.append(b.AbbreviatedDayNames[this.getDay()]);
                    break;
                case "dd":
                    h = true;
                    a.append(d(g(this, 2)));
                    break;
                case "d":
                    h = true;
                    a.append(g(this, 2));
                    break;
                case "MMMM":
                    a.append(b.MonthGenitiveNames && s() ? b.MonthGenitiveNames[g(this, 1)] : b.MonthNames[g(this, 1)]);
                    break;
                case "MMM":
                    a.append(b.AbbreviatedMonthGenitiveNames && s() ? b.AbbreviatedMonthGenitiveNames[g(this, 1)] : b.AbbreviatedMonthNames[g(this, 1)]);
                    break;
                case "MM":
                    a.append(d(g(this, 1) + 1));
                    break;
                case "M":
                    a.append(g(this, 1) + 1);
                    break;
                case "yyyy":
                    a.append(v(f ? f[0] : Date._getEraYear(this, b, Date._getEra(this, l), k)));
                    break;
                case "yy":
                    a.append(d((f ? f[0] : Date._getEraYear(this, b, Date._getEra(this, l), k)) % 100));
                    break;
                case "y":
                    a.append((f ? f[0] : Date._getEraYear(this, b, Date._getEra(this, l), k)) % 100);
                    break;
                case "hh":
                    c = this.getHours() % 12;
                    if (c === 0) c = 12;
                    a.append(d(c));
                    break;
                case "h":
                    c = this.getHours() % 12;
                    if (c === 0) c = 12;
                    a.append(c);
                    break;
                case "HH":
                    a.append(d(this.getHours()));
                    break;
                case "H":
                    a.append(this.getHours());
                    break;
                case "mm":
                    a.append(d(this.getMinutes()));
                    break;
                case "m":
                    a.append(this.getMinutes());
                    break;
                case "ss":
                    a.append(d(this.getSeconds()));
                    break;
                case "s":
                    a.append(this.getSeconds());
                    break;
                case "tt":
                    a.append(this.getHours() < 12 ? b.AMDesignator : b.PMDesignator);
                    break;
                case "t":
                    a.append((this.getHours() < 12 ? b.AMDesignator : b.PMDesignator).charAt(0));
                    break;
                case "f":
                    a.append(m(this.getMilliseconds()).charAt(0));
                    break;
                case "ff":
                    a.append(m(this.getMilliseconds()).substr(0, 2));
                    break;
                case "fff":
                    a.append(m(this.getMilliseconds()));
                    break;
                case "z":
                    c = this.getTimezoneOffset() / 60;
                    a.append((c <= 0 ? "+" : "-") + Math.floor(Math.abs(c)));
                    break;
                case "zz":
                    c = this.getTimezoneOffset() / 60;
                    a.append((c <= 0 ? "+" : "-") + d(Math.floor(Math.abs(c))));
                    break;
                case "zzz":
                    c = this.getTimezoneOffset() / 60;
                    a.append((c <= 0 ? "+" : "-") + d(Math.floor(Math.abs(c))) + ":" + d(Math.abs(this.getTimezoneOffset() % 60)));
                    break;
                case "g":
                case "gg":
                    if (b.eras) a.append(b.eras[Date._getEra(this, l) + 1]);
                    break;
                case "/":
                    a.append(b.DateSeparator)
                }
            }
            return a.toString()
        };
        Date._appendPreOrPostMatch = function(e, b) {
            var d = 0,
                a = false;
            for (var c = 0, g = e.length; c < g; c++) {
                var f = e.charAt(c);
                switch (f) {
                case "'":
                    if (a) b.append("'");
                    else d++;
                    a = false;
                    break;
                case "\\":
                    if (a) b.append("\\");
                    a = !a;
                    break;
                default:
                    b.append(f);
                    a = false
                }
            }
            return d
        };
        Date._expandFormat = function(a, b) {
            if (!b) b = "F";
            var c = b.length;
            if (c === 1)
                switch (b) {
                case "d":
                    return a.ShortDatePattern;
                case "D":
                    return a.LongDatePattern;
                case "t":
                    return a.ShortTimePattern;
                case "T":
                    return a.LongTimePattern;
                case "f":
                    return a.LongDatePattern + " " + a.ShortTimePattern;
                case "F":
                    return a.FullDateTimePattern;
                case "M":
                case "m":
                    return a.MonthDayPattern;
                case "s":
                    return a.SortableDateTimePattern;
                case "Y":
                case "y":
                    return a.YearMonthPattern;
                default:
                    throw Error.format(Sys.Res.formatInvalidString)
                }
            else if (c === 2 && b.charAt(0) === "%") b = b.charAt(1);
            return b
        };
        Date._expandYear = function(c, a) {
            var d = new Date,
                e = Date._getEra(d);
            if (a < 100) {
                var b = Date._getEraYear(d, c, e);
                a += b - b % 100;
                if (a > c.Calendar.TwoDigitYearMax) a -= 100
            }
            return a
        };
        Date._getEra = function(e, c) {
            if (!c) return 0;
            var b, d = e.getTime();
            for (var a = 0, f = c.length; a < f; a += 4) {
                b = c[a + 2];
                if (b === null || d >= b) return a
            }
            return 0
        };
        Date._getEraYear = function(d, b, e, c) {
            var a = d.getFullYear();
            if (!c && b.eras) a -= b.eras[e + 3];
            return a
        };
        Date._getParseRegExp = function(b, e) {
            if (!b._parseRegExp) b._parseRegExp = {};
            else if (b._parseRegExp[e]) return b._parseRegExp[e];
            var c = Date._expandFormat(b, e);
            c = c.replace(/([\^\$\.\*\+\?\|\[\]\(\)\{\}])/g, "\\\\$1");
            var a = new Sys.StringBuilder("^"),
                j = [],
                f = 0,
                i = 0,
                h = Date._getTokenRegExp(),
                d;
            while ((d = h.exec(c)) !== null) {
                var l = c.slice(f, d.index);
                f = h.lastIndex;
                i += Date._appendPreOrPostMatch(l, a);
                if (i % 2 === 1) {
                    a.append(d[0]);
                    continue
                }
                switch (d[0]) {
                case "dddd":
                case "ddd":
                case "MMMM":
                case "MMM":
                case "gg":
                case "g":
                    a.append("(\\D+)");
                    break;
                case "tt":
                case "t":
                    a.append("(\\D*)");
                    break;
                case "yyyy":
                    a.append("(\\d{4})");
                    break;
                case "fff":
                    a.append("(\\d{3})");
                    break;
                case "ff":
                    a.append("(\\d{2})");
                    break;
                case "f":
                    a.append("(\\d)");
                    break;
                case "dd":
                case "d":
                case "MM":
                case "M":
                case "yy":
                case "y":
                case "HH":
                case "H":
                case "hh":
                case "h":
                case "mm":
                case "m":
                case "ss":
                case "s":
                    a.append("(\\d\\d?)");
                    break;
                case "zzz":
                    a.append("([+-]?\\d\\d?:\\d{2})");
                    break;
                case "zz":
                case "z":
                    a.append("([+-]?\\d\\d?)");
                    break;
                case "/":
                    a.append("(\\" + b.DateSeparator + ")")
                }
                Array.add(j, d[0])
            }
            Date._appendPreOrPostMatch(c.slice(f), a);
            a.append("$");
            var k = a.toString().replace(/\s+/g, "\\s+"),
                g = {
                    "regExp": k,
                    "groups": j
                };
            b._parseRegExp[e] = g;
            return g
        };
        Date._getTokenRegExp = function() {
            return /\/|dddd|ddd|dd|d|MMMM|MMM|MM|M|yyyy|yy|y|hh|h|HH|H|mm|m|ss|s|tt|t|fff|ff|f|zzz|zz|z|gg|g/g
        };
        Date._parse = function(h, d, i) {
            var a, c, b, f, e, g = false;
            for (a = 1, c = i.length; a < c; a++) {
                f = i[a];
                if (f) {
                    g = true;
                    b = Date._parseExact(h, f, d);
                    if (b) return b
                }
            }
            if (!g) {
                e = d._getDateTimeFormats();
                for (a = 0, c = e.length; a < c; a++) {
                    b = Date._parseExact(h, e[a], d);
                    if (b) return b
                }
            }
            return null
        };
        Date._parseExact = function(w, D, k) {
            w = w.trim();
            var g = k.dateTimeFormat,
                A = Date._getParseRegExp(g, D),
                C = (new RegExp(A.regExp)).exec(w);
            if (C === null) return null;
            var B = A.groups,
                x = null,
                e = null,
                c = null,
                j = null,
                i = null,
                d = 0,
                h,
                q = 0,
                r = 0,
                f = 0,
                n = null,
                v = false;
            for (var t = 0, E = B.length; t < E; t++) {
                var a = C[t + 1];
                if (a)
                    switch (B[t]) {
                    case "dd":
                    case "d":
                        j = parseInt(a, 10);
                        if (j < 1 || j > 31) return null;
                        break;
                    case "MMMM":
                        c = k._getMonthIndex(a);
                        if (c < 0 || c > 11) return null;
                        break;
                    case "MMM":
                        c = k._getAbbrMonthIndex(a);
                        if (c < 0 || c > 11) return null;
                        break;
                    case "M":
                    case "MM":
                        c = parseInt(a, 10) - 1;
                        if (c < 0 || c > 11) return null;
                        break;
                    case "y":
                    case "yy":
                        e = Date._expandYear(g, parseInt(a, 10));
                        if (e < 0 || e > 9999) return null;
                        break;
                    case "yyyy":
                        e = parseInt(a, 10);
                        if (e < 0 || e > 9999) return null;
                        break;
                    case "h":
                    case "hh":
                        d = parseInt(a, 10);
                        if (d === 12) d = 0;
                        if (d < 0 || d > 11) return null;
                        break;
                    case "H":
                    case "HH":
                        d = parseInt(a, 10);
                        if (d < 0 || d > 23) return null;
                        break;
                    case "m":
                    case "mm":
                        q = parseInt(a, 10);
                        if (q < 0 || q > 59) return null;
                        break;
                    case "s":
                    case "ss":
                        r = parseInt(a, 10);
                        if (r < 0 || r > 59) return null;
                        break;
                    case "tt":
                    case "t":
                        var z = a.toUpperCase();
                        v = z === g.PMDesignator.toUpperCase();
                        if (!v && z !== g.AMDesignator.toUpperCase()) return null;
                        break;
                    case "f":
                        f = parseInt(a, 10) * 100;
                        if (f < 0 || f > 999) return null;
                        break;
                    case "ff":
                        f = parseInt(a, 10) * 10;
                        if (f < 0 || f > 999) return null;
                        break;
                    case "fff":
                        f = parseInt(a, 10);
                        if (f < 0 || f > 999) return null;
                        break;
                    case "dddd":
                        i = k._getDayIndex(a);
                        if (i < 0 || i > 6) return null;
                        break;
                    case "ddd":
                        i = k._getAbbrDayIndex(a);
                        if (i < 0 || i > 6) return null;
                        break;
                    case "zzz":
                        var u = a.split(/:/);
                        if (u.length !== 2) return null;
                        h = parseInt(u[0], 10);
                        if (h < -12 || h > 13) return null;
                        var o = parseInt(u[1], 10);
                        if (o < 0 || o > 59) return null;
                        n = h * 60 + (a.startsWith("-") ? -o : o);
                        break;
                    case "z":
                    case "zz":
                        h = parseInt(a, 10);
                        if (h < -12 || h > 13) return null;
                        n = h * 60;
                        break;
                    case "g":
                    case "gg":
                        var p = a;
                        if (!p || !g.eras) return null;
                        p = p.toLowerCase().trim();
                        for (var s = 0, F = g.eras.length; s < F; s += 4)
                            if (p === g.eras[s + 1].toLowerCase()) {
                                x = s;
                                break
                            }
                        if (x === null) return null
                    }
            }
            var b = new Date,
                l,
                m = g.Calendar.convert;
            if (m) l = m.fromGregorian(b);
            if (!m) l = [b.getFullYear(), b.getMonth(), b.getDate()];
            if (e === null) e = l[0];
            else if (g.eras) e += g.eras[(x || 0) + 3];
            if (c === null) c = l[1];
            if (j === null) j = l[2];
            if (m) {
                b = m.toGregorian(e, c, j);
                if (b === null) return null
            } else {
                b.setFullYear(e, c, j);
                if (b.getDate() !== j) return null;
                if (i !== null && b.getDay() !== i) return null
            }
            if (v && d < 12) d += 12;
            b.setHours(d, q, r, f);
            if (n !== null) {
                var y = b.getMinutes() - (n + b.getTimezoneOffset());
                b.setHours(b.getHours() + parseInt(y / 60, 10), y % 60)
            }
            return b
        };
    }
    if (typeof Date.prototype.fromJSON !== "function") {
        Date.fromJSON = function(json) {
            return new Date(parseInt(json.substr(6)));
        };
    }
    if (typeof Date.prototype.getDayOrdinal !== "function") {
        Date.prototype.getDayOrdinal = function() {
            var day = this.getDay();
            return day.toOrdinal();
        };
    }


// Number extensions
    if (typeof Number.prototype.format !== "function" && typeof CultureInfo !== "undefined") {
        Number.prototype.format = function(a) {
            return this._toFormattedString(a, CultureInfo.InvariantCulture)
        };
        Number.prototype.localeFormat = function(a) {
            return this._toFormattedString(a, CultureInfo.CurrentCulture)
        };
        Number.prototype._toFormattedString = function(e, j) {
            if (!e || e.length === 0 || e === "i")
                if (j && j.name.length > 0) return this.toLocaleString();
                else return this.toString();
            var o = ["n %", "n%", "%n"],
                n = ["-n %", "-n%", "-%n"],
                p = ["(n)", "-n", "- n", "n-", "n -"],
                m = ["$n", "n$", "$ n", "n $"],
                l = ["($n)", "-$n", "$-n", "$n-", "(n$)", "-n$", "n-$", "n$-", "-n $", "-$ n", "n $-", "$ n-", "$ -n", "n- $", "($ n)", "(n $)"];

            function g(a, c, d) {
                for (var b = a.length; b < c; b++) a = d ? "0" + a : a + "0";
                return a
            }

            function i(j, i, l, n, p) {
                var h = l[0],
                    k = 1,
                    o = Math.pow(10, i),
                    m = Math.round(j * o) / o;
                if (!isFinite(m)) m = j;
                j = m;
                var b = j.toString(),
                    a = "",
                    c,
                    e = b.split(/e/i);
                b = e[0];
                c = e.length > 1 ? parseInt(e[1]) : 0;
                e = b.split(".");
                b = e[0];
                a = e.length > 1 ? e[1] : "";
                var q;
                if (c > 0) {
                    a = g(a, c, false);
                    b += a.slice(0, c);
                    a = a.substr(c)
                } else if (c < 0) {
                    c = -c;
                    b = g(b, c + 1, true);
                    a = b.slice(-c, b.length) + a;
                    b = b.slice(0, -c)
                }
                if (i > 0) {
                    if (a.length > i) a = a.slice(0, i);
                    else a = g(a, i, false);
                    a = p + a
                } else a = "";
                var d = b.length - 1,
                    f = "";
                while (d >= 0) {
                    if (h === 0 || h > d)
                        if (f.length > 0) return b.slice(0, d + 1) + n + f + a;
                        else return b.slice(0, d + 1) + a;
                    if (f.length > 0) f = b.slice(d - h + 1, d + 1) + n + f;
                    else f = b.slice(d - h + 1, d + 1);
                    d -= h;
                    if (k < l.length) {
                        h = l[k];
                        k++
                    }
                }
                return b.slice(0, d + 1) + n + f + a
            }

            var a = j.numberFormat,
                d = Math.abs(this);
            if (!e) e = "D";
            var b = -1;
            if (e.length > 1) b = parseInt(e.slice(1), 10);
            var c;
            switch (e.charAt(0)) {
            case "d":
            case "D":
                c = "n";
                if (b !== -1) d = g("" + d, b, true);
                if (this < 0) d = -d;
                break;
            case "c":
            case "C":
                if (this < 0) c = l[a.CurrencyNegativePattern];
                else c = m[a.CurrencyPositivePattern];
                if (b === -1) b = a.CurrencyDecimalDigits;
                d = i(Math.abs(this), b, a.CurrencyGroupSizes, a.CurrencyGroupSeparator, a.CurrencyDecimalSeparator);
                break;
            case "n":
            case "N":
                if (this < 0) c = p[a.NumberNegativePattern];
                else c = "n";
                if (b === -1) b = a.NumberDecimalDigits;
                d = i(Math.abs(this), b, a.NumberGroupSizes, a.NumberGroupSeparator, a.NumberDecimalSeparator);
                break;
            case "p":
            case "P":
                if (this < 0) c = n[a.PercentNegativePattern];
                else c = o[a.PercentPositivePattern];
                if (b === -1) b = a.PercentDecimalDigits;
                d = i(Math.abs(this) * 100, b, a.PercentGroupSizes, a.PercentGroupSeparator, a.PercentDecimalSeparator);
                break;
            default:
                throw "Format specifier was invalid.";
            }
            var k = /n|\$|-|%/g,
                f = "";
            for (; true;) {
                var q = k.lastIndex,
                    h = k.exec(c);
                f += c.slice(q, h ? h.index : c.length);
                if (!h) break;
                switch (h[0]) {
                case "n":
                    f += d;
                    break;
                case "$":
                    f += a.CurrencySymbol;
                    break;
                case "-":
                    if (/[1-9]/.test(d)) f += a.NegativeSign;
                    break;
                case "%":
                    f += a.PercentSymbol
                }
            }
            return f
        };
        Number.parseLocale = function(a) {
            return Number._parse(a, Sys.CultureInfo.CurrentCulture)
        };
        Number.parseInvariant = function(a) {
            return Number._parse(a, Sys.CultureInfo.InvariantCulture)
        };
        Number._parse = function(b, o) {
            b = b.trim();
            if (b.match(/^[+-]?infinity$/i)) return parseFloat(b);
            if (b.match(/^0x[a-f0-9]+$/i)) return parseInt(b);
            var a = o.numberFormat,
                g = Number._parseNumberNegativePattern(b, a, a.NumberNegativePattern),
                h = g[0],
                e = g[1];
            if (h === "" && a.NumberNegativePattern !== 1) {
                g = Number._parseNumberNegativePattern(b, a, 1);
                h = g[0];
                e = g[1]
            }
            if (h === "") h = "+";
            var j, d, f = e.indexOf("e");
            if (f < 0) f = e.indexOf("E");
            if (f < 0) {
                d = e;
                j = null
            } else {
                d = e.substr(0, f);
                j = e.substr(f + 1)
            }
            var c, k, m = d.indexOf(a.NumberDecimalSeparator);
            if (m < 0) {
                c = d;
                k = null
            } else {
                c = d.substr(0, m);
                k = d.substr(m + a.NumberDecimalSeparator.length)
            }
            c = c.split(a.NumberGroupSeparator).join("");
            var n = a.NumberGroupSeparator.replace(/\u00A0/g, " ");
            if (a.NumberGroupSeparator !== n) c = c.split(n).join("");
            var l = h + c;
            if (k !== null) l += "." + k;
            if (j !== null) {
                var i = Number._parseNumberNegativePattern(j, a, 1);
                if (i[0] === "") i[0] = "+";
                l += "e" + i[0] + i[1]
            }
            if (l.match(/^[+-]?\d*\.?\d*(e[+-]?\d+)?$/)) return parseFloat(l);
            return Number.NaN
        };
        Number._parseNumberNegativePattern = function(a, d, e) {
            var b = d.NegativeSign,
                c = d.PositiveSign;
            switch (e) {
            case 4:
                b = " " + b;
                c = " " + c;
            case 3:
                if (a.endsWith(b)) return ["-", a.substr(0, a.length - b.length)];
                else if (a.endsWith(c)) return ["+", a.substr(0, a.length - c.length)];
                break;
            case 2:
                b += " ";
                c += " ";
            case 1:
                if (a.startsWith(b)) return ["-", a.substr(b.length)];
                else if (a.startsWith(c)) return ["+", a.substr(c.length)];
                break;
            case 0:
                if (a.startsWith("(") && a.endsWith(")")) return ["-", a.substr(1, a.length - 2)]
            }
            return ["", a]
        };
    }
    if (typeof Number.prototype.toOrdinal !== "function") {
        Number.prototype.toOrdinal = function() {
            var n = this % 100;
            var suff = ["th", "st", "nd", "rd", "th"]; // suff for suffix
            var ord = n < 21 ? (n < 4 ? suff[n] : suff[0]) : (n % 10 > 4 ? suff[0] : suff[n % 10]);
            return this + ord;
        };
    }
    if (typeof Number.prototype.pad !== "function") {
        Number.prototype.pad = function(width, pad) {
            pad = pad || '0';
            var value = this + '';
            return value.length >= width ? value : new Array(width - value.length + 1).join(pad) + value;
        };
    }


// String extensions
    if (typeof String.format !== "function" && typeof CultureInfo !== "undefined") {
        String.prototype.format = function() {
            return this._toFormattedString(false, arguments)
        };
        String.prototype.localeFormat = function() {
            return this._toFormattedString(true, arguments)
        };
        String.prototype._toFormattedString = function(l, j) {
            var c = "",
                e = this.toString();
            for (var a = 0; true;) {
                var f = e.indexOf("{", a),
                    d = e.indexOf("}", a);
                if (f < 0 && d < 0) {
                    c += e.slice(a);
                    break
                }
                if (d > 0 && (d < f || f < 0)) {
                    c += e.slice(a, d + 1);
                    a = d + 2;
                    continue
                }
                c += e.slice(a, f);
                a = f + 1;
                if (e.charAt(a) === "{") {
                    c += "{";
                    a++;
                    continue
                }
                if (d < 0) break;
                var h = e.substring(a, d),
                    g = h.indexOf(":"),
                    k = parseInt(g < 0 ? h : h.substring(0, g), 10),
                    i = g < 0 ? "" : h.substring(g + 1),
                    b = j[k];

                if (typeof b === "undefined" || b === null) b = "";
                if (b.toFormattedString) c += b.toFormattedString(i);
                else if (l && b.localeFormat) c += b.localeFormat(i);
                else if (b.format) c += b.format(i);
                else c += b.toString();
                a = d + 1
            }
            return c
        };
    }
    if (typeof String.prototype.pad !== "function") {
        String.prototype.pad = function(width, pad) {
            pad = pad || '0';
            var value = this + '';
            return value.length >= width ? value : new Array(width - value.length + 1).join(pad) + value;
        };
    }
    if (typeof String.prototype.trim !== "function") {
        String.prototype.trim = function() {
            return this.replace(/^\s+|\s+$/g, "");
        };
    }
    if (typeof String.prototype.trimEnd !== "function") {
        String.prototype.trimEnd = function() {
            return this.replace(/\s+$/, "")
        };
    }
    if (typeof String.prototype.trimStart !== "function") {
        String.prototype.trimStart = function() {
            return this.replace(/^\s+/, "")
        };
    }
    if (typeof String.prototype.contains !== 'function') {
        String.prototype.contains = function(str) {
            return this.indexOf(str) != -1;
        };
    }
    if (typeof String.prototype.startsWith !== 'function') {
        String.prototype.startsWith = function(a) {
            return this.substr(0, a.length) === a;
        };
    }
    if (typeof String.prototype.endsWith !== 'function') {
        String.prototype.endsWith = function(a) {
            return this.substr(this.length - a.length) === a;
        };
    }
    if (typeof String.prototype.replaceAll !== 'function') {
        String.prototype.replaceAll = function(str1, str2, ignore) {
            return this.replace(new RegExp(str1.replace(/([\/\,\!\\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\-\&])/g, "\\$&"), (ignore ? "gi" : "g")), (typeof (str2) == "string") ? str2.replace(/\$/g, "$$$$") : str2);
        };
    }
    if (typeof String.prototype.htmlEncode !== 'function') {
        String.prototype.htmlEncode = function() {
            return this
                .replaceAll('&', '&amp;')
                .replaceAll('"', '&quot;')
                .replaceAll("'", '&#39;')
                .replaceAll('<', '&lt;')
                .replaceAll('>', '&gt;');
        };
    }
    if (typeof String.prototype.htmlDecode !== 'function') {
        String.prototype.htmlDecode = function() {
            return this
                .replaceAll('&quot;', '"')
                .replaceAll('&#39;', "'")
                .replaceAll('&lt;', '<')
                .replaceAll('&gt;', '>')
                .replaceAll('&amp;', '&');
        };
    }
    if (typeof String.prototype.toBoolean !== "function") {
        String.prototype.toBoolean = function() {
            var string = this || '';
            switch (string.toLowerCase()) {
            case "true":
            case "yes":
            case "1":
                return true;
            case "false":
            case "no":
            case "0":
            case null:
                return false;
            default:
                return Boolean(string);
            }
        }
    }


    /* jQuery Extensions */
    require(["jquery"], function($) {
        $(document).on("ajaxSend", function (event, xhr, options) {
            if (options.method === "POST") options.headers["__RequestVerificationToken"] = $('[name="__RequestVerificationToken"]:first').val();
        });

        // Selector for broken links
        $.extend($.expr[':'], {
            broken: function (selector) {
                var href = $(selector).attr('href');
                var isBroken =
                    href == null
                    || href == undefined
                    || href == ''
                    || href == 'javascript:;'
                    || href == '#';

                return isBroken;
            }
        });

        // Serialize forms and other objects to JSON
        $.fn.serializeObject = function () {
            var o = {};
            var a = this.serializeArray();
            if (a.length == 0) a = this.find(':input').serializeArray();
            $.each(a, function () {
                if (o[this.name] !== undefined) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });
            return o;
        };
    });
});