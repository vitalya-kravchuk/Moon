var Validation = 
{
    OnInvalidCssClass: 'EditError',

    RequiredFields: function(fields)
    {
        var requiredFields = false;
        for (var i = fields.length - 1; i >= 0; i--)
        {
            var field = document.getElementById(fields[i]);
            var emptyField = (field.value == '');
            requiredFields = requiredFields || emptyField;
            if (emptyField)
            {
                this.SetCssClass(field, this.OnInvalidCssClass);
                field.setAttribute('onchange', 'Validation.SetCssClass(this, "")');
                field.focus();
            }
			else
			{
				this.SetCssClass(field, '');
			}
        }
        return requiredFields;
    },
    
    CompareValidator: function(elemId1, elemId2)
    {
        var elem1 = document.getElementById(elemId1);
        var elem2 = document.getElementById(elemId2);
        var compareValid = (elem1.value == elem2.value);
        if (compareValid)
        {
            this.SetCssClass(elem1, '');
            this.SetCssClass(elem2, '');
        }
        else
        {
            this.SetCssClass(elem1, this.OnInvalidCssClass);
            this.SetCssClass(elem2, this.OnInvalidCssClass);
            elem1.select();
        }
        return compareValid;
    },
    
    EmailValidator: function(elemId)
    {
        var element = document.getElementById(elemId);
        var emailValid = this.EmailValid(element.value);
		if (emailValid)
		{
		    this.SetCssClass(element, '');
		}
		else
		{
		    this.SetCssClass(element, this.OnInvalidCssClass);
			element.select();
		}
		return emailValid;
    },
    
    // Helper
    SetCssClass: function(element, cssClass)
    {
        element.setAttribute('class', cssClass);
        element.className = cssClass;
    },
    
    EmailValid: function(str)
    {
        var at = "@";
        var dot = ".";
        var lat = str.indexOf(at);
        var lstr = str.length;
        var ldot = str.indexOf(dot);
     
        if (str.indexOf(at) == -1) return false; 
        if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) return false; 
        if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) return false; 
        if (str.indexOf(at, (lat+1)) != -1) return false; 
        if (str.substring(lat-1, lat) == dot || str.substring(lat+1, lat+2) == dot) return false; 
        if (str.indexOf(dot, (lat+2)) == -1) return false;         
        if (str.indexOf(" ") != -1) return false; 

        return true; 
    }
}
