$.validator.addMethod("checkatleastone",
    function (value, element, param) {
        let checkedCheckboxes = $(element).closest("#answers-container").find("input:checkbox:checked");
        if (checkedCheckboxes.length == 0) {
            return false;
        }

       
        return true;
    });

$.validator.unobtrusive.adapters.addBool("checkatleastone");
