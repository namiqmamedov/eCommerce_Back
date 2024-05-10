const accSingleTriggers = document.querySelectorAll('.js-acc-single-trigger');

accSingleTriggers.forEach(trigger => trigger.addEventListener('click', toggleAccordion));

function toggleAccordion() {
  const currentItem = this.parentNode;
  const isOpen = currentItem.classList.contains('is-open');
  
  if (isOpen) {
    currentItem.classList.remove('is-open');
    currentItem.querySelector('.accordion-single-content').style.maxHeight = null;
  } else {
    currentItem.classList.add('is-open');
    currentItem.querySelector('.accordion-single-content').style.maxHeight = currentItem.querySelector('.accordion-single-content').scrollHeight + 'px';
  }
}
