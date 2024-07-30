
$(document).ready(function() {

  $('.btn-mobile').click(function() {
    $(this).toggleClass('is-open');
    $('body').toggleClass('nav__is-open');
  });
  $('.btn-lang-switch').click(function() {
    $('.lang-list').toggleClass('hidden');
  });
  $('.btn-subnav').click(function(e) {
    e.preventDefault();
    $('.sub-nav').toggleClass('hidden');
  });
  
  //hide on click outside
  $(document).click(function(event) {
    if (!$(event.target).closest('.lang-list').length && !$(event.target).closest('.btn-lang-switch').length && !$(event.target).closest('.btn-subnav').length) {
      $('.lang-list, .sub-nav').addClass('hidden');
    }
  });
});
