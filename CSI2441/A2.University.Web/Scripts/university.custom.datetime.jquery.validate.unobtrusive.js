// fix date validation for chrome, see: http://www.codeproject.com/Tips/579279/Fixing-jQuery-non-US-Date-Validation-for-Chrome
// ISSUE: Editing student in Chrome causes Student Dob to reset, does not pass current value
jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        var isChrome = window.chrome;
        // make correction for chrome
        if (isChrome) {
            var d = new Date();
            return this.optional(element) ||
            !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
        }
            // leave default behavior
        else {
            return this.optional(element) ||
            !/Invalid|NaN/.test(new Date(value));
        }
    }
});