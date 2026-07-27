// Navigation functionality
document.addEventListener('DOMContentLoaded', function() {
    // Smooth scrolling for navigation links
    const navLinks = document.querySelectorAll('.nav-link');
    const sections = document.querySelectorAll('.content-section');
    const scrollTopBtn = document.getElementById('scroll-top');

    // Handle navigation link clicks
    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            
            // Remove active class from all links
            navLinks.forEach(l => l.classList.remove('active'));
            
            // Add active class to clicked link
            this.classList.add('active');
            
            // Get target section
            const targetId = this.getAttribute('href');
            const targetSection = document.querySelector(targetId);
            
            // Scroll to section with offset for better viewing
            if (targetSection) {
                const offsetTop = targetSection.offsetTop - 20;
                window.scrollTo({
                    top: offsetTop,
                    behavior: 'smooth'
                });
            }
        });
    });

    // Highlight active section on scroll
    function highlightNavigation() {
        let current = '';
        
        sections.forEach(section => {
            const sectionTop = section.offsetTop;
            const sectionHeight = section.clientHeight;
            
            if (window.pageYOffset >= sectionTop - 100) {
                current = section.getAttribute('id');
            }
        });

        navLinks.forEach(link => {
            link.classList.remove('active');
            if (link.getAttribute('href') === `#${current}`) {
                link.classList.add('active');
            }
        });
    }

    // Show/hide scroll to top button
    function toggleScrollTopButton() {
        if (window.pageYOffset > 300) {
            scrollTopBtn.classList.add('visible');
        } else {
            scrollTopBtn.classList.remove('visible');
        }
    }

    // Scroll to top functionality
    scrollTopBtn.addEventListener('click', function() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    // Debounce function for performance
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    // Event listeners for scroll
    const debouncedHighlight = debounce(highlightNavigation, 50);
    const debouncedToggleBtn = debounce(toggleScrollTopButton, 50);

    window.addEventListener('scroll', function() {
        debouncedHighlight();
        debouncedToggleBtn();
    });

    // Initial calls
    highlightNavigation();
    toggleScrollTopButton();

    // Add copy functionality to code blocks
    const codeBlocks = document.querySelectorAll('.code-block');
    
    codeBlocks.forEach(block => {
        // Create copy button
        const copyButton = document.createElement('button');
        copyButton.className = 'copy-btn';
        copyButton.textContent = 'Copy';
        copyButton.style.cssText = `
            position: absolute;
            top: 10px;
            right: 10px;
            padding: 0.5rem 1rem;
            background: var(--primary-color);
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 0.85rem;
            transition: all 0.2s;
            opacity: 0;
        `;
        
        // Make parent relative
        block.style.position = 'relative';
        block.appendChild(copyButton);
        
        // Show button on hover
        block.addEventListener('mouseenter', () => {
            copyButton.style.opacity = '1';
        });
        
        block.addEventListener('mouseleave', () => {
            copyButton.style.opacity = '0';
        });
        
        // Copy functionality
        copyButton.addEventListener('click', async () => {
            const code = block.querySelector('code').textContent;
            try {
                await navigator.clipboard.writeText(code);
                copyButton.textContent = 'Copied!';
                copyButton.style.background = 'var(--success)';
                
                setTimeout(() => {
                    copyButton.textContent = 'Copy';
                    copyButton.style.background = 'var(--primary-color)';
                }, 2000);
            } catch (err) {
                console.error('Failed to copy code:', err);
                copyButton.textContent = 'Failed';
                copyButton.style.background = 'var(--warning)';
                
                setTimeout(() => {
                    copyButton.textContent = 'Copy';
                    copyButton.style.background = 'var(--primary-color)';
                }, 2000);
            }
        });
    });

    // Add table of contents expansion for mobile
    if (window.innerWidth <= 768) {
        const sidebar = document.querySelector('.sidebar');
        const navMenu = document.querySelector('.nav-menu');
        
        // Create toggle button for mobile
        const toggleBtn = document.createElement('button');
        toggleBtn.className = 'nav-toggle';
        toggleBtn.innerHTML = '☰ Menu';
        toggleBtn.style.cssText = `
            width: 100%;
            padding: 1rem;
            background: var(--surface-light);
            color: var(--text-primary);
            border: none;
            border-bottom: 1px solid var(--border);
            cursor: pointer;
            font-size: 1rem;
            text-align: left;
        `;
        
        sidebar.insertBefore(toggleBtn, navMenu);
        navMenu.style.display = 'none';
        
        toggleBtn.addEventListener('click', () => {
            if (navMenu.style.display === 'none') {
                navMenu.style.display = 'block';
                toggleBtn.innerHTML = '✕ Close';
            } else {
                navMenu.style.display = 'none';
                toggleBtn.innerHTML = '☰ Menu';
            }
        });
        
        // Close menu when link is clicked on mobile
        navLinks.forEach(link => {
            link.addEventListener('click', () => {
                if (window.innerWidth <= 768) {
                    navMenu.style.display = 'none';
                    toggleBtn.innerHTML = '☰ Menu';
                }
            });
        });
    }

    // Add animation to cards on scroll
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, observerOptions);

    // Observe all cards
    document.querySelectorAll('.card').forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        card.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
        observer.observe(card);
    });

    // Add keyboard navigation
    document.addEventListener('keydown', function(e) {
        // Alt + Arrow Up: scroll to top
        if (e.altKey && e.key === 'ArrowUp') {
            e.preventDefault();
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        }
        
        // Alt + Arrow Down: scroll to bottom
        if (e.altKey && e.key === 'ArrowDown') {
            e.preventDefault();
            window.scrollTo({
                top: document.body.scrollHeight,
                behavior: 'smooth'
            });
        }
    });

    // Search functionality (basic filtering)
    const searchInput = document.createElement('input');
    searchInput.type = 'text';
    searchInput.placeholder = 'Search documentation...';
    searchInput.style.cssText = `
        width: 100%;
        padding: 0.75rem;
        margin-bottom: 1rem;
        background: var(--surface-light);
        color: var(--text-primary);
        border: 1px solid var(--border);
        border-radius: 8px;
        font-size: 0.95rem;
    `;
    
    const sidebar = document.querySelector('.sidebar');
    const logo = document.querySelector('.logo');
    sidebar.insertBefore(searchInput, logo.nextSibling);

    searchInput.addEventListener('input', function(e) {
        const searchTerm = e.target.value.toLowerCase();
        
        navLinks.forEach(link => {
            const text = link.textContent.toLowerCase();
            const listItem = link.parentElement;
            
            if (text.includes(searchTerm)) {
                listItem.style.display = 'block';
            } else {
                listItem.style.display = 'none';
            }
        });
    });

    // Console easter egg
    console.log('%c2D Platformer Toolkit', 'color: #6366f1; font-size: 24px; font-weight: bold;');
    console.log('%cDocumentation loaded successfully!', 'color: #22d3ee; font-size: 14px;');
    console.log('%cBuilt with ❤️ by Peykarimeh', 'color: #94a3b8; font-size: 12px;');
});
