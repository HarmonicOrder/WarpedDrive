(function($) {
$(window).on("load", function(){
  var $sourceImg = $('.title img');
  var glitchedImg = new Image(), $glitchedImg = null;
  $glitchedImg = $(glitchedImg).appendTo($('.title')).addClass('.glitched');
  var quality = 50, normalImageTimeFloor = 2000, normalImageTimeRange = 1000, glitchImageTimeFloor = 200, glitchImageTimeRange = 400;
  
  function getRandom100()
  {
      return Math.floor(Math.random()*100);
  }
  
  function showNewGlitch()
  {
    quality += getRandom100();
    quality = quality % 100;
    return glitch({
        seed: getRandom100(),
        quality: quality
    }).fromImage($sourceImg[0]).toDataURL().then(function(dataURL){
        glitchedImg.src = dataURL;
        $sourceImg.hide();
        $glitchedImg.show();
        return;
    });      
  }
  
  function restore(){
      $glitchedImg.hide();
      $sourceImg.show();
  }
  
  function randomGlitch(){
      showNewGlitch().then(function(){
         setTimeout(function() {
             restore();
             setTimeout(function() {
                 randomGlitch();
                 normalImageTimeFloor = 500; //after first run through, use a lower floor
             }, normalImageTimeFloor + Math.random()*normalImageTimeRange);
         }, glitchImageTimeFloor + Math.random()*glitchImageTimeRange); 
      });
  }
  
  randomGlitch();
});
})(jQuery)