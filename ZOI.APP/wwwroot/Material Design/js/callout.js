/*
  Developed by LoveTech.io, Copyright 2017
*/

$(document).ready(function() {
	$(document).on("click", ".CalloutBtn", function() {
		var calloutObj = $(this).data("Callout");
		if(calloutObj != null) {
			calloutObj.toggleCallout();
		}
	});
	$(document).on("click", ".calloutCloseBtn", function() {
		var calloutObj = $(this).closest('.callout').data("Callout");
		if(calloutObj != null) {
			calloutObj.hideCallout();
		}
	});
});

function closeAllCallouts() {
	$('.callout').hide();
}

function CalloutObject(el, opts) {
    this.$el  = $(el);
    this.opts = opts;
	if(this.opts == null) {
		  this.opts = {};
	}
	//Defaults
	if(!this.opts.hasOwnProperty("priority")) {
		this.opts.priority = "rbtl";
	}
	if(!this.opts.hasOwnProperty("containment")) {
		this.opts.containment = $('body');
	}
	if(!this.opts.hasOwnProperty("addBtnWidth")) {
		this.opts.addBtnWidth = 0;
	}
	if(!this.opts.hasOwnProperty("addBtnHeight")) {
		this.opts.addBtnHeight = 0;
	}
	if(!this.opts.hasOwnProperty("type")) {
		if(this.$el.closest(".callout-wrapper").length == 0) {
			this.opts.type = "fixed";
		} else {
			this.opts.type = "relative";
		}
	}
	this.first = true;
	//Todo: light/ dark variants
	this.$el.addClass("calloutBtn");
	
	
	if(this.opts.type == "relative") {
		this.wrapper = this.$el.closest(".callout-wrapper");
		var actDims = this.wrapper.actualDims();
		this.wrapper.css("max-width", actDims.width + this.opts.addBtnWidth + "px");
		this.wrapper.css("max-height", actDims.height + this.opts.addBtnHeight + "px");
	}
	this.wrapper = this.$el.closest(".callout-wrapper");
	if(this.wrapper.length == 0) {
		this.wrapper = this.$el;
	}
  }

  
  CalloutObject.prototype.toggleCallout = function() {
	if(this.callout == null || !this.callout.is(":visible")) {
		this.showCallout();
	} else {
		this.hideCallout();
	}
  };
  
	CalloutObject.prototype.randomString = function(lng) {
		var text = "";
		var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

		for( var i=0; i < lng; i++ )
			text += possible.charAt(Math.floor(Math.random() * possible.length));

		return text;
	};
  CalloutObject.prototype.init = function(opts) {
		var id = this.$el.attr("id");
		if(id == null) {
			id = this.randomString(10);
			this.$el.attr("id", id);
		}
		this.id = id;
		if(this.callout == null) {
			var styleProps = [];
			//Z Index Others
			var calloutHtml = ['<div id="' + id + "_callout" + '" class="callout" style="display:none;' + styleProps.join(";") + '">',
								'	<div class="calloutCloseBtn"></div>',
								'	<div class="content">' + this.opts.content + '</div>',
								'</div>'].join("");
			if(this.opts.type == "relative") {
				this.$el.closest(".callout-wrapper").append(calloutHtml);
				this.callout = this.wrapper.find("#" + id + "_callout");
			} else if(this.opts.type == "fixed") {
				this.opts.containment.append(calloutHtml);
				this.callout = this.opts.containment.find("#" + id + "_callout");
			}
			this.callout.data("Callout", this);
			if(this.opts.hasOwnProperty("calloutStyles")) {
				for(var prop in this.opts.calloutStyles) {
					if(this.opts.calloutStyles.hasOwnProperty(prop)) {
						this.callout.css(prop, this.opts.calloutStyles[prop]);
					}
				}
			}
		}
	};
	
    CalloutObject.prototype.refreshCalculations = function() {
		var windowHeight = $(window).height();
		this.wrapperPosition = this.wrapper[0].getBoundingClientRect();
		this.containmentWidth = this.opts.containment.width();
		this.containmentHeight = this.opts.containment.height();
		this.containmentPosition = this.opts.containment[0].getBoundingClientRect();
		this.wrapperPosType = this.wrapper.css("position");
		this.wrapperWidth = this.wrapper.width();
		this.wrapperHeight = this.wrapper.height();
		this.horizontalSpace = this.containmentWidth - this.wrapperWidth;
		var visibleContainmentHeight = this.containmentHeight;
		if(this.containmentPosition.top < 0) {
			visibleContainmentHeight += this.containmentPosition.top;
			if(visibleContainmentHeight > windowHeight) {
				visibleContainmentHeight = windowHeight;
			}
		} else {
			if(this.containmentPosition.top + visibleContainmentHeight > windowHeight) {
				visibleContainmentHeight = windowHeight - this.containmentPosition.top;
			}
		}
		this.containmentHeight = visibleContainmentHeight;
		this.verticalSpace = this.containmentHeight - this.wrapperHeight;
		this.leftSpace = this.wrapperPosition.left - this.containmentPosition.left;
		this.rightSpace = this.horizontalSpace - this.leftSpace - 28;
		this.topSpace = this.wrapperPosition.top - Math.max(0, this.containmentPosition.top);
		this.bottomSpace = this.verticalSpace - this.topSpace;
		
		
		this.callout.find(".content").width(0);
		this.actualWidth = this.callout.find(".content")[0].scrollWidth;
		this.callout.find(".content").width("100%");
	};
    CalloutObject.prototype.sizeAndPositionCallout = function(direction, opts) {
		if(opts == null) {
			opts = {};
		}
		var force = false;
		if(opts.force == true) {
			force = true;
		}
		var deltaL = 0;
		var deltaT = 0;
		if(this.opts.type == "fixed") {
			deltaT = this.wrapperPosition.top;
			deltaL = this.wrapperPosition.left;
		}
		if(direction == null || direction == "b") {
			  this.callout.removeClass("top").removeClass("left").removeClass("right").addClass("bottom");
			  var topAbsPos = this.wrapperHeight + 8;
			  var leftAbsPos = 0;
			  //Width
			  if(this.opts.width != null) {
				  if(!force && (this.containmentWidth < (this.opts.width + 10))) {
					  return false;
				  }
				  var width = Math.min(this.opts.width, this.containmentWidth - 10);
				  this.callout.width(width);
				  this.calloutWidth = width;
				  leftAbsPos = -1 * (width / 2) + (this.wrapperWidth / 2);
			  }
			  
			  //Height
			  if(this.opts.height != null) {
				  if(!force && (this.bottomSpace < (this.opts.height + 10))) {
					  return false;
				  }
				  this.calloutHeight = this.opts.height;
				  this.callout.height(this.opts.height);
			  }  else {
				  //Use the element's height, after width has been set, for text content
					this.callout.find(".content").height(0);
					this.actualHeight = this.callout.find(".content")[0].scrollHeight;
					this.callout.find(".content").height("100%");
					if(!force && (this.bottomSpace < (this.actualHeight + 10))) {
					  return false;
					}
					this.calloutHeight = this.actualHeight;
					this.callout.height(this.actualHeight);
			  }
			  
			  
			  this.callout.css("left", (leftAbsPos+deltaL) + "px");
			  this.callout.css("top",  (topAbsPos+deltaT) + "px");
			  this.realignHorizontally();
			  return true;
		} else if(direction == "t") {
			  this.callout.removeClass("bottom").removeClass("left").removeClass("right").addClass("top");
			  var topAbsPos = 0;
			  var leftAbsPos = 0;
			  //Width
			  if(this.opts.width != null) {
				  if(!force && (this.horizontalSpace < (this.opts.width + 10))) {
					  return false;
				  }
				  var width = Math.min(this.opts.width, this.containmentWidth - 10);
				  this.callout.width(width);
				  this.calloutWidth = width;
				  leftAbsPos = -1 * (width / 2) + (this.wrapperWidth / 2);
			  } 
			  
			  
			  //Height
			  if(this.opts.height != null) {
					if(this.topSpace < (this.opts.height + 15)) {
					  return false;
					}
					this.calloutHeight = this.opts.height;
					this.callout.height(this.opts.height);
					topAbsPos = -1 * this.opts.height - 15;
			  } 
			  /*else if(this.opts.minHeight != null && this.opts.maxHeight != null) {
				  //Todo
			  }*/ 
			  else {
				  //Use the element's height, after width has been set, for text content
					this.callout.find(".content").height(0);
					this.actualHeight = this.callout.find(".content")[0].scrollHeight;
					this.callout.find(".content").height("100%");
					if(!force && (this.topSpace < (this.actualHeight + 15))) {
					  return false;
					}
					this.calloutHeight = this.actualHeight;
					this.callout.height(this.actualHeight);
					topAbsPos = -1 * this.actualHeight - 15;
			  }
			  
			  
			  this.callout.css("left", (leftAbsPos+deltaL) + "px");
			  this.callout.css("top",  (topAbsPos+deltaT) + "px");
			  this.realignHorizontally();
			  return true;
		} else if(direction == "l") {
			  this.callout.removeClass("bottom").removeClass("top").removeClass("right").addClass("left");
			  var topAbsPos = 0;
			  var leftAbsPos = -1 * (this.opts.width + 23);
			  //Width
			  if(this.opts.width != null) {
				  if(!force && (this.leftSpace < (this.opts.width + 10))) {
					  return false;
				  }
				  var width = Math.min(this.leftSpace - 10, this.opts.width);
				  this.callout.width(width);
				  this.calloutWidth = width;
			  } 
			  
			  //Height
			  if(this.opts.height != null) {
				  if(!force && (this.containmentHeight < (this.opts.height + 10))) {
					  return false;
				  }
				  this.callout.height(this.opts.height);
				  this.calloutHeight = this.opts.height;
				  topAbsPos = (-1 * (this.calloutHeight / 2) + (this.wrapperHeight / 2)) - 3;
			  } else {
				  //Use the element's height, after width has been set, for text content
					this.callout.find(".content").height(0);
					this.actualHeight = this.callout.find(".content")[0].scrollHeight;
					this.callout.find(".content").height("100%");
					if(!force && (this.containmentHeight < (this.actualHeight + 10))) {
					  return false;
					}
					this.calloutHeight = this.actualHeight;
					this.callout.height(this.actualHeight);
					topAbsPos = (-1 * (this.calloutHeight / 2) + (this.wrapperHeight / 2)) - 3;
			  }
			  
			  this.callout.css("left", (leftAbsPos+deltaL) + "px");
			  this.callout.css("top",  (topAbsPos+deltaT) + "px");
			  this.realignVertically();
			  return true;
		} else if(direction == "r") {
			  this.callout.removeClass("bottom").removeClass("top").removeClass("left").addClass("right");
			  var topAbsPos = 0;
			  var leftAbsPos  = this.wrapperWidth + 10;
			  //Width
			  if(this.opts.width != null) {
				  if(!force && (this.rightSpace < (this.opts.width + 10))) {
					  return false;
				  }
				  var width = Math.min(this.opts.width, this.rightSpace - 10);
				  this.callout.width(width);
				  this.calloutWidth = this.opts.width;
			  }
			  
			  //Height
			  if(this.opts.height != null) {
				  if(!force && (this.containmentHeight < (this.opts.height + 10))) {
					  return false;
				  }
				  this.calloutHeight = this.opts.height;
				  this.callout.height(this.opts.height);
				  topAbsPos = (-1 * (this.calloutHeight / 2) + (this.wrapperHeight / 2)) -3;
			  } else {
				  //Use the element's height, after width has been set, for text content
					this.callout.find(".content").height(0);
					this.actualHeight = this.callout.find(".content")[0].scrollHeight;
					this.callout.find(".content").height("100%");
					if(!force && (this.containmentHeight < (this.actualHeight + 10))) {
					  return false;
					}
					this.calloutHeight = this.actualHeight;
					this.callout.height(this.actualHeight);
				  topAbsPos = (-1 * (this.calloutHeight / 2) + (this.wrapperHeight / 2)) - 3;
			  }
			  
			  this.callout.css("left", (leftAbsPos+deltaL) + "px");
			  this.callout.css("top",  (topAbsPos+deltaT) + "px");
			  this.realignVertically();
			  return true;
		  }
	    return false;
	};
	

  CalloutObject.prototype.realignVertically = function() {
	  var deltaV = (this.calloutHeight / 2) - (this.wrapperHeight / 2);
	  var caretTop = (this.calloutHeight / 2);
	  //Compare with this.topSpace and this.bottomSpace     ***  5px padding
	  if(deltaV > this.topSpace) {
		  var diffV = deltaV - this.topSpace;
		 //var curCalloutPosition = this.callout[0].getBoundingClientRect();
		  var curCalloutPosition = this.callout.position();
		  this.callout.css("top", curCalloutPosition.top + diffV + 5);
		  caretTop -= diffV + 5;
	  } else if(deltaV > this.bottomSpace) {
		  var diffV = deltaV - this.bottomSpace;
		  //var curCalloutPosition = this.callout[0].getBoundingClientRect();
		  var curCalloutPosition = this.callout.position();
		  this.callout.css("top", curCalloutPosition.top - diffV - 5);
		  caretTop += diffV + 5;
	  }
	  
	  this.setCaretTop(caretTop - 10);
  };

  CalloutObject.prototype.realignHorizontally = function() {
	  var deltaH = (this.calloutWidth / 2) - (this.wrapperWidth / 2);
	  var caretLeft = (this.calloutWidth / 2);
	  //Compare with this.leftSpace and this.rightSpace     ***  5px padding
	  if(deltaH > this.leftSpace) {
		  var diffH = deltaH - this.leftSpace;
		  var curCalloutPosition = this.callout.position();
		  //var curCalloutPosition = this.callout[0].getBoundingClientRect();
		  this.callout.css("left", curCalloutPosition.left + diffH + 5);
		  caretLeft -= diffH + 5;
	  } else if(deltaH > this.rightSpace) {
		  var diffH = deltaH - this.rightSpace;
		 var curCalloutPosition = this.callout.position();
		  //var curCalloutPosition = this.callout[0].getBoundingClientRect();
		  this.callout.css("left", curCalloutPosition.left - diffH - 5);
		  caretLeft += diffH + 5;
	  }
	  
	  this.setCaretLeft(caretLeft - 10);
  };
  CalloutObject.prototype.setCaretLeft = function(left) {
	  $('style#' + this.callout[0].id + '_override').remove();
	$('body').append('<style id="' + this.callout[0].id + '_override">#' +this.callout[0].id + '::before { left:' + left + 'px } </style>');
  };
  CalloutObject.prototype.setCaretTop = function(top) {
	  $('style#' + this.callout[0].id + '_override').remove();
	$('body').append('<style id="' + this.callout[0].id + '_override">#' +this.callout[0].id + '::before { top:' + top + 'px } </style>');
  };
  CalloutObject.prototype.hideCallout = function(opts) {
	  if(this.callout != null) {
		this.callout.fadeOut("fast"); 
	  }
	  if(this.opts.onClose  != null) {
		  this.opts.onClose();
	  }
  };
  CalloutObject.prototype.showCallout = function(opts) {
		//Ensure the element is loaded by now
		this.init();
		this.callout.show();
		this.refreshCalculations();
		var foundPlace = false;
		for(var i = 0; i < this.opts.priority.length; i++) {
			if(this.sizeAndPositionCallout(this.opts.priority[i])) {
				foundPlace = true;
				break;
			}
		}
		if(!foundPlace) {
			var dirScores = [{
				d: "t",
				w: this.containmentWidth,
				h: this.topSpace,
				r: this.containmentWidth / this.topSpace,
				a: this.containmentWidth * this.topSpace,
			}, {
				d: "b",
				w: this.containmentWidth,
				h: this.bottomSpace,
				r: this.containmentWidth / this.bottomSpace,
				a: this.containmentWidth * this.bottomSpace,
			}, {
				d: "l",
				w: this.leftSpace,
				h: this.containmentHeight,
				r: this.leftSpace / this.containmentHeight,
				a: this.containmentHeight * this.leftSpace,
			}, {
				d: "r",
				w: this.rightSpace,
				h: this.containmentHeight,
				r: this.rightSpace / this.containmentHeight,
				a: this.containmentHeight * this.rightSpace,
			}];
			//Want a large area and reasonable ratio
			dirScores.sort(function(a, b) {
				return b.a - a.a;
			});
			for(var i = 0; i < dirScores.length; i++) {
				if(this.sizeAndPositionCallout(dirScores[i].d, {force:true})) {
					foundPlace = true;
					break;
				}
			}
		}
		this.callout.hide();
		this.callout.fadeIn("fast");
		if(this.opts.onOpen  != null) {
		  this.opts.onOpen(this.callout, this.wrapper);
		}
		if(this.first && this.opts.onFirstOpen != null) {
			this.opts.onFirstOpen(this.callout, this.wrapper);
		}
		if(typeof getTopZ == "function") {
			var z = getTopZ();
			this.callout.css("z-index", z);
		}
		this.first = false;
	};
	
  $.fn.Callout = function(opts) {
	  if($(this).length == 1) {
        var object = new CalloutObject(this, opts);
      	$(this).addClass("CalloutBtn").data("Callout", object);
        return object;
     } else {
        $(this).each(function() {
           var object = new CalloutObject(this, opts);
      	   $(this).addClass("CalloutBtn").data("Callout", object);
       });
     }     
  };
  
  
  $.fn.actualDims = function(){
        // find the closest visible parent and get it's hidden children
    var visibleParent = this.closest(':visible').children(),
        thisHeight, thisWidth;
    
	var elsToShowHide = visibleParent.find("*:not(:visible)");
	
    elsToShowHide.show();
    // get the height
    thisHeight = this.height();
    thisWidth = this.width();
    
    elsToShowHide.hide();
    
    return {
		height:thisHeight,
		width:thisWidth
	};
  };