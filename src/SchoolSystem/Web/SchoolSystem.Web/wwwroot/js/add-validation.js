$.validator.addMethod("checkatleastone",
    function (value, element, param) {
        let checkedCheckboxes = $(element).closest("#answers-container").find("input:checkbox:checked");
        if (checkedCheckboxes.length == 0) {
            return false;
        }

       
        return true;
    });

$.validator.addMethod("minvalueonegreaterthanmaxofprev",
    function (value, element, param) {
        let valueSplitted = value.split("-")
        let otherPropertyValueSplitted = $('#' + param).val().split("-")

        let minValue = parseInt(valueSplitted[0]);

       let maxValueOtherProperty = parseInt(otherPropertyValueSplitted[1]);

        if (minValue != maxValueOtherProperty + 1) {
            return false;
        }
        return true;
    });

$.validator.addMethod("maxgreaterthanmin",
    function (value, element, param) {

            let valueSplitted = value.split('-');
            let minValue = parseInt(valueSplitted[0]);
            let maxValue = parseInt(valueSplitted[1]);

        if (maxValue <= minValue) {
            return false;
        }


        return true;
    });

$.validator.addMethod("poorminvaluezero",
    function (value, element, param) {

        let valueSplitted = value.split('-');
        let minValue = parseInt(valueSplitted[0]);

        if (minValue != 0) {
            return false;
        }


        return true;
    });

$.validator.unobtrusive.adapters.addBool("checkatleastone");
$.validator.unobtrusive.adapters.addSingleVal("minvalueonegreaterthanmaxofprev", "otherproperty");
$.validator.unobtrusive.adapters.addBool("maxgreaterthanmin");
$.validator.unobtrusive.adapters.addBool("poorminvaluezero");
