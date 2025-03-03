$("input[type=text]").keypress(function (e) {
  var $this = $(this);
  $(this).val(
    $this
      .val()
      .replace(/(\s{2,})|[^a-zA-Z0-9_@.,-/!#%&?:]/g, " ")
      .replace(/^\s*/, "")
  );
});

$("input[type=text]").focusout(function (e) {
  var $this = $(this);
  $(this).val(
    $this
      .val()
      .replace(/(\s{2,})|[^a-zA-Z0-9_@.,-/!#%&?:]/g, " ")
      .replace(/^\s*/, "")
  );
});

$("textarea").keypress(function (e) {
  var $this = $(this);
  $(this).val(
    $this
      .val()
      .replace(/(\s{2,})|[^a-zA-Z0-9_@.,-/!#%&?:]/g, "")
      .replace(/^\s*/, "")
  );
});

$("textarea").focusout(function (e) {
  var $this = $(this);
  $(this).val(
    $this
      .val()
      .replace(/(\s{2,})|[^a-zA-Z0-9_@.,-/!#%&?:]/g, "")
      .replace(/^\s*/, "")
  );
});
