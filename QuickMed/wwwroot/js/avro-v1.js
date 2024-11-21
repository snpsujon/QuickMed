/*! JS Avro Phonetic v1.1.4 http://omicronlab.com | https://raw.github.com/torifat/jsAvroPhonetic/master/MPL-1.1.txt */
var OmicronLab = {};
OmicronLab.Avro = {};
OmicronLab.Avro.Phonetic = {
    parse: function(input) {
        var fixed = this.fixString(input);
        var output = "";
        for (var cur = 0; cur < fixed.length; ++cur) {
            var start = cur,
                end = cur + 1,
                prev = start - 1;
            var matched = false;
            for (var i = 0; i < this.data.patterns.length; ++i) {
                var pattern = this.data.patterns[i];
                end = cur + pattern.f.length;
                if (end <= fixed.length && fixed.substring(start, end) == pattern.f) {
                    prev = start - 1;
                    if (typeof pattern.u !== "undefined") {
                        for (var j = 0; j < pattern.u.length; ++j) {
                            var rule = pattern.u[j];
                            var r = true;
                            var chk = 0;
                            for (var k = 0; k < rule.m.length; ++k) {
                                var match = rule.m[k];
                                if (match.t === "s") {
                                    chk = end
                                } else {
                                    chk = prev
                                }
                                if (typeof match.negative === "undefined") {
                                    match.negative = false;
                                    if (match.s.charAt(0) === "!") {
                                        match.negative = true;
                                        match.s = match.s.substring(1)
                                    }
                                }
                                if (typeof match.v === "undefined") match.v = "";
                                if (match.s === "p") {
                                    if (!(chk < 0 && match.t === "p" || chk >= fixed.length && match.t === "s" || this.isPunctuation(fixed.charAt(chk))) ^ match.negative) {
                                        r = false;
                                        break
                                    }
                                } else if (match.s === "v") {
                                    if (!((chk >= 0 && match.t === "p" || chk < fixed.length && match.t === "s") && this.isVowel(fixed.charAt(chk))) ^ match.negative) {
                                        r = false;
                                        break
                                    }
                                } else if (match.s === "c") {
                                    if (!((chk >= 0 && match.t === "p" || chk < fixed.length && match.t === "s") && this.isConsonant(fixed.charAt(chk))) ^ match.negative) {
                                        r = false;
                                        break
                                    }
                                } else if (match.s === "e") {
                                    var s, e;
                                    if (match.t === "s") {
                                        s = end;
                                        e = end + match.v.length
                                    } else {
                                        s = start - match.v.length;
                                        e = start
                                    }
                                    if (!this.isExact(match.v, fixed, s, e, match.negative)) {
                                        r = false;
                                        break
                                    }
                                }
                            }
                            if (r) {
                                output += rule.r;
                                cur = end - 1;
                                matched = true;
                                break
                            }
                        }
                    }
                    if (matched == true) break;
                    output += pattern.r;
                    cur = end - 1;
                    matched = true;
                    break
                }
            }
            if (!matched) {
                output += fixed.charAt(cur)
            }
        }
        return output
    },
    fixString: function(input) {
        var fixed = "";
        for (var i = 0; i < input.length; ++i) {
            var cChar = input.charAt(i);
            if (this.isCaseSensitive(cChar)) {
                fixed += cChar
            } else {
                fixed += cChar.toLowerCase()
            }
        }
        return fixed
    },
    isVowel: function(c) {
        return this.data.v.indexOf(c.toLowerCase()) >= 0
    },
    isConsonant: function(c) {
        return this.data.c.indexOf(c.toLowerCase()) >= 0
    },
    isPunctuation: function(c) {
        return !(this.isVowel(c) || this.isConsonant(c))
    },
    isExact: function(needle, heystack, start, end, not) {
        return (start >= 0 && end < heystack.length && heystack.substring(start, end) === needle) ^ not
    },
    isCaseSensitive: function(c) {
        return this.data.casesensitive.indexOf(c.toLowerCase()) >= 0
    },
    data: {
        patterns: [{
			
		
            f: "bhl",
            r: "ভ্ল"
        }, {
            f: "psh",
            r: "পশ"
        }, {
            f: "bdh",
            r: "ব্ধ"
        }, {
            f: "bj",
            r: "ব্জ"
        }, {
            f: "bd",
            r: "ব্দ"
        }, {
            f: "bb",
            r: "ব্ব"
        }, {
            f: "bl",
            r: "ব্ল"
        }, {
            f: "bh",
            r: "ভ"
        }, {
            f: "vl",
            r: "ভ্ল"
        }, {
            f: "b",
            r: "ব"
        }, {
            f: "v",
            r: "ভ"
        }, {
            f: "cNG",
            r: "চ্ঞ"
        }, {
            f: "cch",
            r: "চ্ছ"
        }, {
            f: "cc",
            r: "চ্চ"
        }, {
            f: "ch",
            r: "ছ"
        }, {
            f: "c",
            r: "চ"
        }, {
            f: "dhn",
            r: "ধ্ন"
        }, {
            f: "dhm",
            r: "ধ্ম"
        }, {
            f: "dgh",
            r: "দ্ঘ"
        }, {
            f: "ddh",
            r: "দ্ধ"
        }, {
            f: "dbh",
            r: "দ্ভ"
        }, {
            f: "dv",
            r: "দ্ভ"
        }, {
            f: "dm",
            r: "দ্ম"
        }, {
            f: "DD",
            r: "ড্ড"
        }, {
            f: "Dh",
            r: "ঢ"
        }, {
            f: "dh",
            r: "ধ"
        }, {
            f: "dg",
            r: "দ্গ"
        }, {
            f: "dd",
            r: "দ্দ"
        }, {
            f: "D",
            r: "ড"
        }, {
            f: "d",
            r: "দ"
        }, {
            f: "...",
            r: "..."
        }, {
            f: ".`",
            r: "."
        }, {
            f: "..",
            r: "।।"
        }, {
            f: ".",
            r: "।"
        }, {
            f: "ghn",
            r: "ঘ্ন"
        }, {
            f: "Ghn",
            r: "ঘ্ন"
        }, {
            f: "gdh",
            r: "গ্ধ"
        }, {
            f: "Gdh",
            r: "গ্ধ"
        }, {
            f: "gN",
            r: "গ্ণ"
        }, {
            f: "GN",
            r: "গ্ণ"
        }, {
            f: "gn",
            r: "গ্ন"
        }, {
            f: "Gn",
            r: "গ্ন"
        }, {
            f: "gm",
            r: "গ্ম"
        }, {
            f: "Gm",
            r: "গ্ম"
        }, {
            f: "gl",
            r: "গ্ল"
        }, {
            f: "Gl",
            r: "গ্ল"
        }, {
            f: "gg",
            r: "জ্ঞ"
        }, {
            f: "GG",
            r: "জ্ঞ"
        }, {
            f: "Gg",
            r: "জ্ঞ"
        }, {
            f: "gG",
            r: "জ্ঞ"
        }, {
            f: "gh",
            r: "ঘ"
        }, {
            f: "Gh",
            r: "ঘ"
        }, {
            f: "g",
            r: "গ"
        }, {
            f: "G",
            r: "গ"
        }, {
            f: "hN",
            r: "হ্ণ"
        }, {
            f: "hn",
            r: "হ্ন"
        }, {
            f: "hm",
            r: "হ্ম"
        }, {
            f: "hl",
            r: "হ্ল"
        }, {
            f: "h",
            r: "হ"
        }, {
            f: "jjh",
            r: "জ্ঝ"
        }, {
            f: "jNG",
            r: "জ্ঞ"
        }, {
            f: "jh",
            r: "ঝ"
        }, {
            f: "jj",
            r: "জ্জ"
        }, {
            f: "j",
            r: "জ"
        }, {
            f: "J",
            r: "জ"
        }, {
            f: "kkhN",
            r: "ক্ষ্ণ"
        }, {
            f: "kShN",
            r: "ক্ষ্ণ"
        }, {
            f: "kkhm",
            r: "ক্ষ্ম"
        }, {
            f: "kShm",
            r: "ক্ষ্ম"
        }, {
            f: "kxN",
            r: "ক্ষ্ণ"
        }, {
            f: "kxm",
            r: "ক্ষ্ম"
        }, {
            f: "kkh",
            r: "ক্ষ"
        }, {
            f: "kSh",
            r: "ক্ষ"
        }, {
            f: "ksh",
            r: "কশ"
        }, {
            f: "kx",
            r: "ক্ষ"
        }, {
            f: "kk",
            r: "ক্ক"
        }, {
            f: "kT",
            r: "ক্ট"
        }, {
            f: "kt",
            r: "ক্ত"
        }, {
            f: "kl",
            r: "ক্ল"
        }, {
            f: "ks",
            r: "ক্স"
        }, {
            f: "kh",
            r: "খ"
        }, {
            f: "k",
            r: "ক"
        }, {
            f: "lbh",
            r: "ল্ভ"
        }, {
            f: "ldh",
            r: "ল্ধ"
        }, {
            f: "lkh",
            r: "লখ"
        }, {
            f: "lgh",
            r: "লঘ"
        }, {
            f: "lph",
            r: "লফ"
        }, {
            f: "lk",
            r: "ল্ক"
        }, {
            f: "lg",
            r: "ল্গ"
        }, {
            f: "lT",
            r: "ল্ট"
        }, {
            f: "lD",
            r: "ল্ড"
        }, {
            f: "lp",
            r: "ল্প"
        }, {
            f: "lv",
            r: "ল্ভ"
        }, {
            f: "lm",
            r: "ল্ম"
        }, {
            f: "ll",
            r: "ল্ল"
        }, {
            f: "lb",
            r: "ল্ব"
        }, {
            f: "l",
            r: "ল"
        }, {
            f: "mth",
            r: "ম্থ"
        }, {
            f: "mph",
            r: "ম্ফ"
        }, {
            f: "mbh",
            r: "ম্ভ"
        }, {
            f: "mpl",
            r: "মপ্ল"
        }, {
            f: "mn",
            r: "ম্ন"
        }, {
            f: "mp",
            r: "ম্প"
        }, {
            f: "mv",
            r: "ম্ভ"
        }, {
            f: "mm",
            r: "ম্ম"
        }, {
            f: "ml",
            r: "ম্ল"
        }, {
            f: "mb",
            r: "ম্ব"
        }, {
            f: "mf",
            r: "ম্ফ"
        }, {
            f: "m",
            r: "ম"
        }, {
            f: "0",
            r: "০"
        }, 
		
		
		
		{
            f: "1/2",
            r: "½"
        }, 		
		
	
		{
            f: "1/3",
            r: "⅓"
        },	
		
		
		{
            f: "1/4",
            r: "¼"
        },


		{
            f: "1/4",
            r: "⅔"
        },



		
		
		
		{
            f: "1",
            r: "১"
        }, {
            f: "2",
            r: "২"
        }, {
            f: "3",
            r: "৩"
        }, {
            f: "4",
            r: "৪"
        }, {
            f: "5",
            r: "৫"
        }, {
            f: "6",
            r: "৬"
        }, {
            f: "7",
            r: "৭"
        }, {
            f: "8",
            r: "৮"
        }, {
            f: "9",
            r: "৯"
        }, {
            f: "NgkSh",
            r: "ঙ্ক্ষ"
        }, {
            f: "Ngkkh",
            r: "ঙ্ক্ষ"
        }, {
            f: "NGch",
            r: "ঞ্ছ"
        }, {
            f: "Nggh",
            r: "ঙ্ঘ"
        }, {
            f: "Ngkh",
            r: "ঙ্খ"
        }, {
            f: "NGjh",
            r: "ঞ্ঝ"
        }, {
            f: "ngOU",
            r: "ঙ্গৌ"
        }, {
            f: "ngOI",
            r: "ঙ্গৈ"
        }, {
            f: "Ngkx",
            r: "ঙ্ক্ষ"
        }, {
            f: "NGc",
            r: "ঞ্চ"
        }, {
            f: "nch",
            r: "ঞ্ছ"
        }, {
            f: "njh",
            r: "ঞ্ঝ"
        }, {
            f: "ngh",
            r: "ঙ্ঘ"
        }, {
            f: "Ngk",
            r: "ঙ্ক"
        }, {
            f: "Ngx",
            r: "ঙ্ষ"
        }, {
            f: "Ngg",
            r: "ঙ্গ"
        }, {
            f: "Ngm",
            r: "ঙ্ম"
        }, {
            f: "NGj",
            r: "ঞ্জ"
        }, {
            f: "ndh",
            r: "ন্ধ"
        }, {
            f: "nTh",
            r: "ন্ঠ"
        }, {
            f: "NTh",
            r: "ণ্ঠ"
        }, {
            f: "nth",
            r: "ন্থ"
        }, {
            f: "nkh",
            r: "ঙ্খ"
        }, {
            f: "ngo",
            r: "ঙ্গ"
        }, {
            f: "nga",
            r: "ঙ্গা"
        }, {
            f: "ngi",
            r: "ঙ্গি"
        }, {
            f: "ngI",
            r: "ঙ্গী"
        }, {
            f: "ngu",
            r: "ঙ্গু"
        }, {
            f: "ngU",
            r: "ঙ্গূ"
        }, {
            f: "nge",
            r: "ঙ্গে"
        }, {
            f: "ngO",
            r: "ঙ্গো"
        }, {
            f: "NDh",
            r: "ণ্ঢ"
        }, {
            f: "nsh",
            r: "নশ"
        }, {
            f: "Ngr",
            r: "ঙর"
        }, {
            f: "NGr",
            r: "ঞর"
        }, {
            f: "ngr",
            r: "ংর"
        }, {
            f: "nj",
            r: "ঞ্জ"
        }, {
            f: "Ng",
            r: "ঙ"
        }, {
            f: "NG",
            r: "ঞ"
        }, {
            f: "nk",
            r: "ঙ্ক"
        }, {
            f: "ng",
            r: "ং"
        }, {
            f: "nn",
            r: "ন্ন"
        }, {
            f: "NN",
            r: "ণ্ণ"
        }, {
            f: "Nn",
            r: "ণ্ন"
        }, {
            f: "nm",
            r: "ন্ম"
        }, {
            f: "Nm",
            r: "ণ্ম"
        }, {
            f: "nd",
            r: "ন্দ"
        }, {
            f: "nT",
            r: "ন্ট"
        }, {
            f: "NT",
            r: "ণ্ট"
        }, {
            f: "nD",
            r: "ন্ড"
        }, {
            f: "ND",
            r: "ণ্ড"
        }, {
            f: "nt",
            r: "ন্ত"
        }, {
            f: "ns",
            r: "ন্স"
        }, {
            f: "nc",
            r: "ঞ্চ"
        }, {
            f: "n",
            r: "ন"
        }, {
            f: "N",
            r: "ণ"
        }, {
            f: "OI`",
            r: "ৈ"
        }, {
            f: "OU`",
            r: "ৌ"
        }, {
            f: "O`",
            r: "ো"
        }, {
            f: "OI",
            r: "ৈ",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }],
                r: "ঐ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }],
                r: "ঐ"
            }]
        }, {
            f: "OU",
            r: "ৌ",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }],
                r: "ঔ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }],
                r: "ঔ"
            }]
        }, {
            f: "O",
            r: "ো",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }],
                r: "ও"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }],
                r: "ও"
            }]
        }, {
            f: "phl",
            r: "ফ্ল"
        }, {
            f: "pT",
            r: "প্ট"
        }, {
            f: "pt",
            r: "প্ত"
        }, {
            f: "pn",
            r: "প্ন"
        }, {
            f: "pp",
            r: "প্প"
        }, {
            f: "pl",
            r: "প্ল"
        }, {
            f: "ps",
            r: "প্স"
        }, {
            f: "ph",
            r: "ফ"
        }, {
            f: "fl",
            r: "ফ্ল"
        }, {
            f: "f",
            r: "ফ"
        }, {
            f: "p",
            r: "প"
        }, {
            f: "rri`",
            r: "ৃ"
        }, {
            f: "rri",
            r: "ৃ",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }],
                r: "ঋ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }],
                r: "ঋ"
            }]
        }, {
            f: "rrZ",
            r: "রর‍্য"
        }, {
            f: "rry",
            r: "রর‍্য"
        }, {
            f: "rZ",
            r: "র‍্য",
            u: [{
                m: [{
                    t: "p",
                    s: "c"
                }, {
                    t: "p",
                    s: "!e",
                    v: "r"
                }, {
                    t: "p",
                    s: "!e",
                    v: "y"
                }, {
                    t: "p",
                    s: "!e",
                    v: "w"
                }, {
                    t: "p",
                    s: "!e",
                    v: "x"
                }],
                r: "্র্য"
            }]
        }, {
            f: "ry",
            r: "র‍্য",
            u: [{
                m: [{
                    t: "p",
                    s: "c"
                }, {
                    t: "p",
                    s: "!e",
                    v: "r"
                }, {
                    t: "p",
                    s: "!e",
                    v: "y"
                }, {
                    t: "p",
                    s: "!e",
                    v: "w"
                }, {
                    t: "p",
                    s: "!e",
                    v: "x"
                }],
                r: "্র্য"
            }]
        }, {
            f: "rr",
            r: "রর",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "s",
                    s: "!v"
                }, {
                    t: "s",
                    s: "!e",
                    v: "r"
                }, {
                    t: "s",
                    s: "!p"
                }],
                r: "র্"
            }, {
                m: [{
                    t: "p",
                    s: "c"
                }, {
                    t: "p",
                    s: "!e",
                    v: "r"
                }],
                r: "্রর"
            }]
        }, {
            f: "Rg",
            r: "ড়্গ"
        }, {
            f: "Rh",
            r: "ঢ়"
        }, {
            f: "R",
            r: "ড়"
        }, {
            f: "r",
            r: "র",
            u: [{
                m: [{
                    t: "p",
                    s: "c"
                }, {
                    t: "p",
                    s: "!e",
                    v: "r"
                }, {
                    t: "p",
                    s: "!e",
                    v: "y"
                }, {
                    t: "p",
                    s: "!e",
                    v: "w"
                }, {
                    t: "p",
                    s: "!e",
                    v: "x"
                }, {
                    t: "p",
                    s: "!e",
                    v: "Z"
                }],
                r: "্র"
            }]
        }, {
            f: "shch",
            r: "শ্ছ"
        }, {
            f: "ShTh",
            r: "ষ্ঠ"
        }, {
            f: "Shph",
            r: "ষ্ফ"
        }, {
            f: "Sch",
            r: "শ্ছ"
        }, {
            f: "skl",
            r: "স্ক্ল"
        }, {
            f: "skh",
            r: "স্খ"
        }, {
            f: "sth",
            r: "স্থ"
        }, {
            f: "sph",
            r: "স্ফ"
        }, {
            f: "shc",
            r: "শ্চ"
        }, {
            f: "sht",
            r: "শ্ত"
        }, {
            f: "shn",
            r: "শ্ন"
        }, {
            f: "shm",
            r: "শ্ম"
        }, {
            f: "shl",
            r: "শ্ল"
        }, {
            f: "Shk",
            r: "ষ্ক"
        }, {
            f: "ShT",
            r: "ষ্ট"
        }, {
            f: "ShN",
            r: "ষ্ণ"
        }, {
            f: "Shp",
            r: "ষ্প"
        }, {
            f: "Shf",
            r: "ষ্ফ"
        }, {
            f: "Shm",
            r: "ষ্ম"
        }, {
            f: "spl",
            r: "স্প্ল"
        }, {
            f: "sk",
            r: "স্ক"
        }, {
            f: "Sc",
            r: "শ্চ"
        }, {
            f: "sT",
            r: "স্ট"
        }, {
            f: "st",
            r: "স্ত"
        }, {
            f: "sn",
            r: "স্ন"
        }, {
            f: "sp",
            r: "স্প"
        }, {
            f: "sf",
            r: "স্ফ"
        }, {
            f: "sm",
            r: "স্ম"
        }, {
            f: "sl",
            r: "স্ল"
        }, {
            f: "sh",
            r: "শ"
        }, {
            f: "Sc",
            r: "শ্চ"
        }, {
            f: "St",
            r: "শ্ত"
        }, {
            f: "Sn",
            r: "শ্ন"
        }, {
            f: "Sm",
            r: "শ্ম"
        }, {
            f: "Sl",
            r: "শ্ল"
        }, {
            f: "Sh",
            r: "ষ"
        }, {
            f: "s",
            r: "স"
        }, {
            f: "S",
            r: "শ"
        }, {
            f: "oo`",
            r: "ু"
        }, {
            f: "oo",
            r: "ু",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "উ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "উ"
            }]
        }, {
            f: "o`",
            r: ""
        }, {
            f: "oZ",
            r: "অ্য"
        }, {
            f: "o",
            r: "",
            u: [{
                m: [{
                    t: "p",
                    s: "v"
                }, {
                    t: "p",
                    s: "!e",
                    v: "o"
                }],
                r: "ও"
            }, {
                m: [{
                    t: "p",
                    s: "v"
                }, {
                    t: "p",
                    s: "e",
                    v: "o"
                }],
                r: "অ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }],
                r: "অ"
            }]
        }, {
            f: "tth",
            r: "ত্থ"
        }, {
            f: "t``",
            r: "ৎ"
        }, {
            f: "TT",
            r: "ট্ট"
        }, {
            f: "Tm",
            r: "ট্ম"
        }, {
            f: "Th",
            r: "ঠ"
        }, {
            f: "tn",
            r: "ত্ন"
        }, {
            f: "tm",
            r: "ত্ম"
        }, {
            f: "th",
            r: "থ"
        }, {
            f: "tt",
            r: "ত্ত"
        }, {
            f: "T",
            r: "ট"
        }, {
            f: "t",
            r: "ত"
        }, {
            f: "aZ",
            r: "অ্যা"
        }, {
            f: "AZ",
            r: "অ্যা"
        }, {
            f: "a`",
            r: "া"
        }, {
            f: "A`",
            r: "া"
        }, {
            f: "a",
            r: "া",
            u: [{
                m: [{
                    t: "p",
                    s: "p"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "আ"
            }, {
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "p",
                    s: "!e",
                    v: "a"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "য়া"
            }, {
                m: [{
                    t: "p",
                    s: "e",
                    v: "a"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "আ"
            }]
        }, {
            f: "i`",
            r: "ি"
        }, {
            f: "i",
            r: "ি",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "ই"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "ই"
            }]
        }, {
            f: "I`",
            r: "ী"
        }, {
            f: "I",
            r: "ী",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "ঈ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "ঈ"
            }]
        }, {
            f: "u`",
            r: "ু"
        }, {
            f: "u",
            r: "ু",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "উ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "উ"
            }]
        }, {
            f: "U`",
            r: "ূ"
        }, {
            f: "U",
            r: "ূ",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "ঊ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "ঊ"
            }]
        }, {
            f: "ee`",
            r: "ী"
        }, {
            f: "ee",
            r: "ী",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "ঈ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "ঈ"
            }]
        }, {
            f: "e`",
            r: "ে"
        }, {
            f: "e",
            r: "ে",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "এ"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }, {
                    t: "s",
                    s: "!e",
                    v: "`"
                }],
                r: "এ"
            }]
        }, {
            f: "z",
            r: "য"
        }, {
            f: "Z",
            r: "্য"
        }, {
            f: "y",
            r: "্য",
            u: [{
                m: [{
                    t: "p",
                    s: "!c"
                }, {
                    t: "p",
                    s: "!p"
                }],
                r: "য়"
            }, {
                m: [{
                    t: "p",
                    s: "p"
                }],
                r: "ইয়"
            }]
        }, {
            f: "Y",
            r: "য়"
        }, {
            f: "q",
            r: "ক"
        }, {
            f: "w",
            r: "ও",
            u: [{
                m: [{
                    t: "p",
                    s: "p"
                }, {
                    t: "s",
                    s: "v"
                }],
                r: "ওয়"
            }, {
                m: [{
                    t: "p",
                    s: "c"
                }],
                r: "্ব"
            }]
        }, {
            f: "x",
            r: "ক্স",
            u: [{
                m: [{
                    t: "p",
                    s: "p"
                }],
                r: "এক্স"
            }]
        }, {
            f: ":`",
            r: ":"
        }, {
            f: ":",
            r: "ঃ"
        }, {
            f: "^`",
            r: "^"
        }, {
            f: "^",
            r: "ঁ"
        }, {
            f: ",,",
            r: "্‌"
        }, {
            f: ",",
            r: ","
        }, {
            f: "$",
            r: "৳"
        }, {
            f: "`",
            r: ""
        }],
        v: "aeiou",
        c: "bcdfghjklmnpqrstvwxyz",
        casesensitive: "oiudgjnrstyz"
    }
};
(function($) {
    var methods = {
        init: function(options, callback) {
            var defaults = {
                bangla: true
            };
            if (options) {
                $.extend(defaults, options)
            }
            return this.each(function() {
                if ("bangla" in this) {
                    return
                }
                this.bangla = defaults.bangla;
                this.callback = callback || $.noop;
                $(this).bind("keydown.avro", methods.keydown);
                $(this).bind("notify.avro", methods.notify);
                $(this).bind("switch.avro", methods.switchKb);
                $(this).bind("focus.avro", methods.focus);
                $(this).bind("ready.avro", methods.ready);
                $(this).trigger("ready")
            })
        },
        notify: function(e) {
            this.callback(this.bangla)
        },
        switchKb: function(e, state) {
            if (typeof state === "undefined") {
                state = !this.bangla
            }
            this.bangla = state;
            $(this).trigger("notify")
        },
        focus: function(e) {
            $(this).trigger("notify")
        },
        ready: function(e) {
            $(this).trigger("notify")
        },
        destroy: function() {
            return this.each(function() {
                $(this).unbind(".avro")
            })
        },
        keydown: function(e) {
            var keycode = e.which;
            if (keycode === 77 && e.ctrlKey && !e.altKey && !e.shiftKey) {
                $(this).trigger("switch", [!this.bangla]);
                return false
            }
            if (!this.bangla) {
                return
            }
            if (keycode === 32 || keycode === 13 || keycode === 9) {
                methods.replace(this)
            }
        },
        replace: function(el) {
            var cur = methods.getCaret(el);
            var last = methods.findLast(el, cur);
            var bangla = OmicronLab.Avro.Phonetic.parse(el.value.substring(last, cur));
            if (document.selection) {
                var range = document.selection.createRange();
                range.moveStart("character", -1 * Math.abs(cur - last));
                range.text = bangla;
                range.collapse(true)
            } else {
                el.value = el.value.substring(0, last) + bangla + el.value.substring(cur);
                el.selectionStart = el.selectionEnd = cur - (Math.abs(cur - last) - bangla.length)
            }
        },
        findLast: function(el, cur) {
            var last = cur - 1;
            while (last > 0) {
                var c = el.value.charAt(last);
                if ($.trim(c) === "") {
                    break
                }
                last--
            }
            return last
        },
        getCaret: function(el) {
            if (el.selectionStart) {
                return el.selectionStart
            } else if (document.selection) {
                el.focus();
                var r = document.selection.createRange();
                if (r === null) {
                    return 0
                }
                var re = el.createTextRange(),
                    rc = re.duplicate();
                re.moveToBookmark(r.getBookmark());
                rc.setEndPoint("EndToStart", re);
                return rc.text.length
            }
            return 0
        }
    };
    $.fn.avro = function(method) {
        if (method in ["init", "destroy"]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1))
        } else if (typeof method === "object" || !method) {
            return methods.init.apply(this, arguments)
        } else {
            $.error("Method " + method + " does not exist on jQuery.avro")
        }
    }
})(jQuery);