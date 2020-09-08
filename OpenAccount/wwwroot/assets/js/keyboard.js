                $('#keyboard').jkeyboard({
                  layout: "numbers_only",
                  input: $('#input_field_1')
                });
        
        
                $(".form-control").focus(function() {
                    $('#keyboard').jkeyboard("setInput", this);
                });