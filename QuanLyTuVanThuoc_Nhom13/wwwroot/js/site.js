setTimeout(() => document.querySelectorAll('.toast').forEach(x => x.remove()), 4200);

const menuButton = document.querySelector('.mobile-menu');
const sidebar = document.querySelector('.sidebar');
menuButton?.addEventListener('click', () => sidebar?.classList.toggle('open'));
document.addEventListener('click', event => {
    if (window.innerWidth <= 1100 && sidebar?.classList.contains('open') &&
        !sidebar.contains(event.target) && !menuButton?.contains(event.target)) {
        sidebar.classList.remove('open');
    }
});
