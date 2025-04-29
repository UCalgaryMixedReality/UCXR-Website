// Core animation and interaction library
document.addEventListener('DOMContentLoaded', function() {
  // Particle background system for the header
  const particleCanvas = document.createElement('canvas');
  particleCanvas.id = 'particle-canvas';
  particleCanvas.style.position = 'absolute';
  particleCanvas.style.top = '0';
  particleCanvas.style.left = '0';
  particleCanvas.style.width = '100%';
  particleCanvas.style.height = '100%';
  particleCanvas.style.pointerEvents = 'none';
  
  // Add the canvas as the first child of the header
  const header = document.querySelector('article#main > header');
  if (header) {
    header.style.position = 'relative';
    header.style.overflow = 'hidden';
    header.insertBefore(particleCanvas, header.firstChild);
    initParticles(particleCanvas);
  }
  
  // 3D card effects for project cards
  initProjectCards();
  
  // Advanced modal with 3D transitions
  initAdvancedModal();
  
  // Interactive timeline for project updates
  createInteractiveTimeline();
});

// Particle system for header background
function initParticles(canvas) {
  const ctx = canvas.getContext('2d');
  let particles = [];
  const maxParticles = 50;
  
  // Resize handler
  function resizeCanvas() {
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;
  }
  
  // Initial size and resize listener
  resizeCanvas();
  window.addEventListener('resize', resizeCanvas);
  
  // Create particles
  function createParticles() {
    for (let i = 0; i < maxParticles; i++) {
      particles.push({
        x: Math.random() * canvas.width,
        y: Math.random() * canvas.height,
        radius: Math.random() * 2 + 1,
        color: `rgba(0, 188, 212, ${Math.random() * 0.5 + 0.25})`,
        speedX: Math.random() * 1 - 0.5,
        speedY: Math.random() * 1 - 0.5,
        connected: []
      });
    }
  }
  
  // Draw particles and connections
  function drawParticles() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    
    // Update positions
    particles.forEach((particle, index) => {
      // Move particles
      particle.x += particle.speedX;
      particle.y += particle.speedY;
      
      // Bounce off edges
      if (particle.x < 0 || particle.x > canvas.width) particle.speedX *= -1;
      if (particle.y < 0 || particle.y > canvas.height) particle.speedY *= -1;
      
      // Draw particle
      ctx.beginPath();
      ctx.arc(particle.x, particle.y, particle.radius, 0, Math.PI * 2);
      ctx.fillStyle = particle.color;
      ctx.fill();
      
      // Connect particles within range
      particle.connected = [];
      for (let j = index + 1; j < particles.length; j++) {
        const otherParticle = particles[j];
        const dx = particle.x - otherParticle.x;
        const dy = particle.y - otherParticle.y;
        const distance = Math.sqrt(dx * dx + dy * dy);
        
        if (distance < 100) {
          particle.connected.push(j);
          ctx.beginPath();
          ctx.moveTo(particle.x, particle.y);
          ctx.lineTo(otherParticle.x, otherParticle.y);
          ctx.strokeStyle = `rgba(0, 188, 212, ${0.2 * (1 - distance / 100)})`;
          ctx.lineWidth = 0.5;
          ctx.stroke();
        }
      }
    });
    
    requestAnimationFrame(drawParticles);
  }
  
  createParticles();
  drawParticles();
}

// 3D perspective effects for project cards
function initProjectCards() {
  const cards = document.querySelectorAll('.status-card');
  
  cards.forEach(card => {
    // Add perspective to each card
    card.style.perspective = '1000px';
    card.style.transformStyle = 'preserve-3d';
    
    card.addEventListener('mousemove', e => {
      const rect = card.getBoundingClientRect();
      const x = e.clientX - rect.left; // x position within the element
      const y = e.clientY - rect.top; // y position within the element
      
      // Calculate rotation based on mouse position
      const centerX = rect.width / 2;
      const centerY = rect.height / 2;
      const rotateY = ((x - centerX) / centerX) * 10; // max 10 degrees
      const rotateX = ((centerY - y) / centerY) * 10; // max 10 degrees
      
      // Apply the transform
      card.style.transform = `rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
      
      // Add dynamic reflection/highlight
      const highlight = `radial-gradient(circle at ${x}px ${y}px, rgba(255,255,255,0.2) 0%, rgba(255,255,255,0) 60%)`;
      card.style.backgroundImage = highlight;
    });
    
    card.addEventListener('mouseleave', () => {
      // Reset transform and remove highlight
      card.style.transform = 'rotateX(0deg) rotateY(0deg)';
      card.style.backgroundImage = 'none';
      
      // Add smooth transition when leaving
      card.style.transition = 'transform 0.5s ease, box-shadow 0.5s ease, background-image 0.5s ease';
      
      // Remove transition after animation completes
      setTimeout(() => {
        card.style.transition = '';
      }, 500);
    });
    
    // Add 3D depth to card elements
    const content = card.querySelector('.status-content');
    if(content) {
      content.style.transform = 'translateZ(20px)';
      content.style.transformStyle = 'preserve-3d';
    }
  });
}

// Advanced modal system with animations and 3D effects
function initAdvancedModal() {
  // Remove the existing modal
  const oldModal = document.querySelector('.modal');
  if (oldModal) {
    oldModal.remove();
  }
  
  // Create new modal with advanced features
  const modal = document.createElement('div');
  modal.className = 'modal';
  modal.style.display = 'none';
  modal.style.position = 'fixed';
  modal.style.zIndex = '1000';
  modal.style.left = '0';
  modal.style.top = '0';
  modal.style.width = '100%';
  modal.style.height = '100%';
  modal.style.backgroundColor = 'rgba(10, 14, 20, 0.95)';
  modal.style.overflow = 'auto';
  modal.style.perspective = '1000px';
  
  // Create modal content with 3D effects
  modal.innerHTML = `
    <div class="modal-content" style="
      background-color: #1a1f2b;
      margin: 5% auto;
      padding: 20px;
      width: 80%;
      max-width: 800px;
      border-radius: 12px;
      box-shadow: 0 15px 30px rgba(0,0,0,0.5);
      transform-style: preserve-3d;
      transform: rotateX(10deg) scale(0.9);
      opacity: 0;
      transition: transform 0.5s ease, opacity 0.5s ease;
    ">
      <span class="modal-close" style="
        color: #e0e0e0;
        float: right;
        font-size: 28px;
        font-weight: bold;
        cursor: pointer;
      ">&times;</span>
      
      <div class="modal-header" style="margin-bottom: 20px;">
        <h3 style="color: #ffe082; margin-top: 0;"></h3>
      </div>
      
      <div class="modal-body" style="display: flex; flex-direction: column; gap: 20px;">
        <div class="modal-image-container" style="overflow: hidden; border-radius: 8px;">
          <img src="" alt="Project Image" style="width: 100%; display: block; border-radius: 8px;">
        </div>
        
        <div class="modal-details">
          <p class="modal-description" style="margin-bottom: 20px;"></p>
          
          <div class="modal-progress">
            <div class="progress-label" style="display: flex; justify-content: space-between; margin-bottom: 5px;">
              <span>Progress</span>
              <span class="progress-percent">0%</span>
            </div>
            <div class="progress-bar-container" style="
              height: 8px;
              background-color: rgba(255,255,255,0.1);
              border-radius: 4px;
              overflow: hidden;
            ">
              <div class="progress-bar" style="
                height: 100%;
                background-color: #00bcd4;
                width: 0%;
                transition: width 1s ease;
              "></div>
            </div>
          </div>
        </div>
      </div>
      
      <div class="modal-timeline" style="
        margin-top: 30px;
        padding-top: 20px;
        border-top: 1px solid rgba(255,255,255,0.1);
      ">
        <h4 style="color: #ffe082; margin-top: 0;">Project Timeline</h4>
        <div class="timeline-container" style="
          display: flex;
          justify-content: space-between;
          margin-top: 20px;
          position: relative;
        ">
          <div class="timeline-line" style="
            position: absolute;
            height: 3px;
            background-color: rgba(255,255,255,0.2);
            top: 15px;
            width: 100%;
            z-index: 1;
          "></div>
        </div>
      </div>
    </div>
  `;
  
  document.body.appendChild(modal);
  
  // Add modal functionality
  const cards = document.querySelectorAll('.status-card');
  const modalContent = modal.querySelector('.modal-content');
  const modalTitle = modal.querySelector('.modal-header h3');
  const modalDescription = modal.querySelector('.modal-description');
  const modalImg = modal.querySelector('.modal-image-container img');
  const closeBtn = modal.querySelector('.modal-close');
  const progressBar = modal.querySelector('.progress-bar');
  const progressPercent = modal.querySelector('.progress-percent');
  const timelineContainer = modal.querySelector('.timeline-container');
  
  // Project data (we'll use this for the expanded modal)
  const projectData = {
    'Optics System': {
      image: 'https://via.placeholder.com/800x400?text=Optics+System',
      description: 'Our custom optics system uses waveguide technology with a 110° field of view. The final prototype includes holographic optical elements to reduce distortion and enhance light efficiency. Currently in calibration phase to optimize clarity and reduce chromatic aberration.',
      progress: 85,
      timeline: [
        { date: 'Jan 2024', milestone: 'Initial design' },
        { date: 'Mar 2024', milestone: 'Prototype v1' },
        { date: 'Jun 2024', milestone: 'Waveguide integration' },
        { date: 'Aug 2024', milestone: 'Final prototype' },
        { date: 'Current', milestone: 'Calibration' }
      ]
    },
    'PCB & Power': {
      image: 'https://via.placeholder.com/800x400?text=PCB+and+Power',
      description: 'Custom circuitry optimized for low power consumption while maintaining high processing capability. Our team has designed a proprietary battery management system that extends runtime and handles thermal challenges specific to head-mounted displays.',
      progress: 70,
      timeline: [
        { date: 'Feb 2024', milestone: 'Circuit design' },
        { date: 'Apr 2024', milestone: 'PCB v1' },
        { date: 'Jul 2024', milestone: 'BMS implementation' },
        { date: 'Current', milestone: 'PCB rev 2 fabrication' },
        { date: 'Upcoming', milestone: 'Integration testing' }
      ]
    },
    'Software & Interaction': {
      image: 'https://via.placeholder.com/800x400?text=Software+and+Interaction',
      description: 'Unity-based framework with custom shaders optimized for augmented reality rendering. Our interaction system combines gaze control with gesture recognition using embedded cameras and IMU sensors for precise spatial mapping.',
      progress: 60,
      timeline: [
        { date: 'Mar 2024', milestone: 'Unity framework' },
        { date: 'May 2024', milestone: 'Custom shaders' },
        { date: 'Jul 2024', milestone: 'UI implementation' },
        { date: 'Current', milestone: 'Gesture recognition' },
        { date: 'Upcoming', milestone: 'Spatial mapping' }
      ]
    },
    'Mechanical Housing': {
      image: 'https://via.placeholder.com/800x400?text=Mechanical+Housing',
      description: 'Carbon fiber composite chassis designed for optimal weight distribution and thermal management. The ergonomic design was refined through multiple iterations of user testing to ensure comfort during extended use sessions.',
      progress: 75,
      timeline: [
        { date: 'Jan 2024', milestone: 'Initial design' },
        { date: 'Apr 2024', milestone: 'First prototype' },
        { date: 'Jun 2024', milestone: 'User testing' },
        { date: 'Aug 2024', milestone: 'Refinement' },
        { date: 'Current', milestone: 'Carbon fiber production' }
      ]
    },
    'Live Demo': {
      image: 'https://via.placeholder.com/800x400?text=Live+Demo',
      description: 'Preparing comprehensive demonstration of our AR headset with focus on medical education applications. The showcase will feature interactive anatomical models with real-time manipulation and cross-sectional viewing.',
      progress: 40,
      timeline: [
        { date: 'May 2024', milestone: 'Demo planning' },
        { date: 'Jul 2024', milestone: 'Content creation' },
        { date: 'Current', milestone: 'Educational sim development' },
        { date: 'Upcoming', milestone: 'Beta testing' },
        { date: 'Fall 2025', milestone: 'Public showcase' }
      ]
    }
  };
  
  // Replace card click handlers with enhanced modal functionality
  cards.forEach(card => {
    card.addEventListener('click', () => {
      const title = card.querySelector('h4').textContent;
      const project = projectData[title] || {
        image: 'https://via.placeholder.com/800x400',
        description: card.querySelector('.status-details').textContent,
        progress: 50,
        timeline: []
      };
      
      // Populate modal with project data
      modalTitle.textContent = title;
      modalImg.src = project.image;
      modalDescription.textContent = project.description;
      
      // Set progress bar
      progressBar.style.width = '0%';
      progressPercent.textContent = '0%';
      
      // Clear previous timeline
      while (timelineContainer.children.length > 1) {
        timelineContainer.removeChild(timelineContainer.lastChild);
      }
      
      // Create timeline points
      project.timeline.forEach((item, index) => {
        const point = document.createElement('div');
        point.className = 'timeline-point';
        point.style.position = 'relative';
        point.style.zIndex = '2';
        point.style.flex = '1';
        point.style.display = 'flex';
        point.style.flexDirection = 'column';
        point.style.alignItems = 'center';
        
        const dot = document.createElement('div');
        dot.style.width = '12px';
        dot.style.height = '12px';
        dot.style.borderRadius = '50%';
        dot.style.backgroundColor = index === project.timeline.length - 1 ? '#00bcd4' : '#ffe082';
        dot.style.marginBottom = '8px';
        
        const label = document.createElement('div');
        label.style.fontSize = '0.8rem';
        label.style.textAlign = 'center';
        label.style.transform = 'translateY(20px)';
        label.style.opacity = '0';
        label.style.transition = 'transform 0.3s ease, opacity 0.3s ease';
        label.innerHTML = `<strong>${item.date}</strong><br>${item.milestone}`;
        
        point.appendChild(dot);
        point.appendChild(label);
        timelineContainer.appendChild(point);
        
        // Hover effect for timeline points
        point.addEventListener('mouseenter', () => {
          label.style.transform = 'translateY(0)';
          label.style.opacity = '1';
        });
        
        point.addEventListener('mouseleave', () => {
          label.style.transform = 'translateY(20px)';
          label.style.opacity = '0';
        });
      });
      
      // Show modal with animation
      modal.style.display = 'block';
      setTimeout(() => {
        modalContent.style.transform = 'rotateX(0deg) scale(1)';
        modalContent.style.opacity = '1';
      }, 10);
      
      // Animate progress bar after modal is shown
      setTimeout(() => {
        progressBar.style.width = `${project.progress}%`;
        progressPercent.textContent = `${project.progress}%`;
      }, 500);
    });
  });
  
  // Close modal functionality
  closeBtn.addEventListener('click', closeModal);
  window.addEventListener('click', e => {
    if (e.target === modal) closeModal();
  });
  
  function closeModal() {
    modalContent.style.transform = 'rotateX(10deg) scale(0.9)';
    modalContent.style.opacity = '0';
    
    setTimeout(() => {
      modal.style.display = 'none';
    }, 500);
  }
}

// Interactive timeline for project roadmap
function createInteractiveTimeline() {
  const timelineSection = document.createElement('section');
  timelineSection.className = 'wrapper style5';
  timelineSection.innerHTML = `
    <div class="inner">
      <header>
        <h2>Development Roadmap</h2>
        <p>Our path from concept to reality</p>
      </header>
      
      <div id="interactive-timeline" style="
        position: relative;
        margin: 100px 0;
        height: 300px;
      ">
        <div class="timeline-line" style="
          position: absolute;
          width: 100%;
          height: 4px;
          background-color: rgba(0, 188, 212, 0.3);
          top: 50%;
          transform: translateY(-50%);
        "></div>
      </div>
    </div>
  `;
  
  // Insert timeline before footer
  const footer = document.querySelector('#footer');
  if (footer) {
    document.querySelector('#page-wrapper').insertBefore(timelineSection, footer);
  }
  
  // Timeline data
  const timelineData = [
    { 
      date: 'Q1 2024', 
      title: 'Concept & Design', 
      description: 'Initial concept development and component selection. Established core design principles and technical requirements.'
    },
    { 
      date: 'Q2 2024', 
      title: 'Prototype Development', 
      description: 'First hardware prototypes and optics integration. Basic software framework established.'
    },
    { 
      date: 'Q3 2024', 
      title: 'Integration', 
      description: 'Hardware and software integration. Initial user testing and feedback collection.'
    },
    { 
      date: 'Q4 2024', 
      title: 'Refinement', 
      description: 'System optimization based on testing. Design refinements and improvements.'
    },
    { 
      date: 'Current', 
      title: 'Advanced Development', 
      description: 'Final hardware revisions. Advanced software features and content development.'
    },
    { 
      date: 'Q3 2025', 
      title: 'Pre-Launch', 
      description: 'Final testing and quality assurance. Preparing educational content and demos.'
    },
    { 
      date: 'Fall 2025', 
      title: 'Public Showcase', 
      description: 'Official public demonstration of the ARmed headset with educational simulations.'
    }
  ];
  
  const timeline = document.getElementById('interactive-timeline');
  
  // Create timeline points
  timelineData.forEach((item, index) => {
    const point = document.createElement('div');
    point.className = 'timeline-point';
    point.style.position = 'absolute';
    point.style.top = '50%';
    point.style.left = `${(index / (timelineData.length - 1)) * 100}%`;
    point.style.transform = 'translate(-50%, -50%)';
    point.style.cursor = 'pointer';
    
    // Current point styling
    const isCurrent = item.date === 'Current';
    
    const dot = document.createElement('div');
    dot.style.width = isCurrent ? '24px' : '16px';
    dot.style.height = isCurrent ? '24px' : '16px';
    dot.style.borderRadius = '50%';
    dot.style.backgroundColor = isCurrent ? '#00bcd4' : '#ffe082';
    dot.style.boxShadow = isCurrent ? '0 0 15px rgba(0, 188, 212, 0.8)' : 'none';
    dot.style.transform = 'translateY(-50%)';
    dot.style.transition = 'all 0.3s ease';
    dot.style.zIndex = '2';
    
    const label = document.createElement('div');
    label.style.position = 'absolute';
    label.style.width = '150px';
    label.style.textAlign = 'center';
    label.style.left = '50%';
    label.style.transform = 'translateX(-50%)';
    label.style.color = isCurrent ? '#00bcd4' : '#ffe082';
    label.style.fontWeight = isCurrent ? 'bold' : 'normal';
    label.style.transition = 'all 0.3s ease';
    
    // Alternate labels above and below timeline
    if (index % 2 === 0) {
      label.style.bottom = '30px';
    } else {
      label.style.top = '30px';
    }
    
    label.textContent = item.date;
    
    // Info card that appears on hover/click
    const infoCard = document.createElement('div');
    infoCard.className = 'timeline-info-card';
    infoCard.style.position = 'absolute';
    infoCard.style.width = '250px';
    infoCard.style.backgroundColor = '#1a1f2b';
    infoCard.style.borderRadius = '8px';
    infoCard.style.padding = '15px';
    infoCard.style.boxShadow = '0 10px 25px rgba(0,0,0,0.5)';
    infoCard.style.zIndex = '10';
    infoCard.style.opacity = '0';
    infoCard.style.visibility = 'hidden';
    infoCard.style.transition = 'all 0.3s ease';
    infoCard.style.transform = 'translateY(20px)';
    infoCard.style.left = '50%';
    infoCard.style.marginLeft = '-125px'; // Center the card
    
    // Position card above or below timeline
    if (index % 2 === 0) {
      infoCard.style.bottom = '60px';
    } else {
      infoCard.style.top = '60px';
    }
    
    infoCard.innerHTML = `
      <h4 style="margin-top: 0; color: #ffe082;">${item.title}</h4>
      <p style="margin-bottom: 0;">${item.description}</p>
    `;
    
    // Append elements
    point.appendChild(dot);
    point.appendChild(label);
    point.appendChild(infoCard);
    timeline.appendChild(point);
    
    // Show info card on hover
    point.addEventListener('mouseenter', () => {
      infoCard.style.opacity = '1';
      infoCard.style.visibility = 'visible';
      infoCard.style.transform = 'translateY(0)';
      dot.style.transform = 'scale(1.2) translateY(-50%)';
      label.style.fontWeight = 'bold';
    });
    
    point.addEventListener('mouseleave', () => {
      infoCard.style.opacity = '0';
      infoCard.style.visibility = 'hidden';
      infoCard.style.transform = 'translateY(20px)';
      if (!isCurrent) {
        dot.style.transform = 'scale(1) translateY(-50%)';
        label.style.fontWeight = 'normal';
      }
    });
  });
  
  // Add scroll-based animation to timeline
  window.addEventListener('scroll', () => {
    const timeline = document.getElementById('interactive-timeline');
    if (!timeline) return;
    
    const rect = timeline.getBoundingClientRect();
    const inView = rect.top < window.innerHeight && rect.bottom >= 0;
    
    if (inView) {
      const points = timeline.querySelectorAll('.timeline-point');
      const progress = Math.min(1, Math.max(0, (window.innerHeight - rect.top) / (window.innerHeight + rect.height)));
      
      points.forEach((point, index) => {
        const pointProgress = (index + 1) / points.length;
        if (progress >= pointProgress) {
          point.querySelector('div').style.transform = 'scale(1) translateY(-50%)';
          point.style.opacity = '1';
        } else {
          point.querySelector('div').style.transform = 'scale(0.5) translateY(-50%)';
          point.style.opacity = '0.5';
        }
      });
    }
  });
}

// Add scroll animations for page elements
document.addEventListener('DOMContentLoaded', function() {
  // Fade in elements as they come into view
  const fadeElements = document.querySelectorAll('.wrapper, h2, h3, h4, p');
  
  // Create an IntersectionObserver to detect when elements are in view
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.style.opacity = '1';
        entry.target.style.transform = 'translateY(0)';
        observer.unobserve(entry.target);
      }
    });
  }, { threshold: 0.1 });
  
  // Set initial state and observe each element
  fadeElements.forEach(el => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(20px)';
    el.style.transition = 'opacity 0.8s ease, transform 0.8s ease';
    observer.observe(el);
  });
  
  // Add parallax effect to header
  const header = document.querySelector('article#main > header');
  if (header) {
    window.addEventListener('scroll', () => {
      const scrollPosition = window.scrollY;
      header.style.backgroundPosition = `50% ${50 + scrollPosition * 0.1}%`;
    });
  }
});

// Add AR visualization demo in header
function addARDemo() {
  const header = document.querySelector('article#main > header');
  if (!header) return;
  
  // Create canvas for AR visualization
  const canvas = document.createElement('canvas');
  canvas.id = 'ar-demo-canvas';
  canvas.style.position = 'absolute';
  canvas.style.top = '0';
  canvas.style.left = '0';
  canvas.style.width = '100%';
  canvas.style.height = '100%';
  canvas.style.pointerEvents = 'none';
  canvas.style.zIndex = '1';
  
  header.appendChild(canvas);
  
  // Initialize AR demo
  const ctx = canvas.getContext('2d');
  let width, height;
  
  function resizeCanvas() {
    width = canvas.width = canvas.offsetWidth;
    height = canvas.height = canvas.offsetHeight;
  }
  
  window.addEventListener('resize', resizeCanvas);
  resizeCanvas();
  
  // Simple AR wireframe object
  const cube = {
    points: [
      {x: -50, y: -50, z: -50},
      {x: 50, y: -50, z: -50},
      {x: 50, y: 50, z: -50},
      {x: -50, y: 50, z: -50},
      {x: -50, y: -50, z: 50},
      {x: 50, y: -50, z: 50},
      {x: 50, y: 50, z: 50},
      {x: -50, y: 50, z: 50}
    ],
    edges: [
      [0, 1], [1, 2], [2, 3], [3, 0],
      [4, 5], [5, 6], [6, 7], [7, 4],
      [0, 4], [1, 5], [2, 6], [3, 7]
    ],
    rotation: {x: 0, y: 0, z: 0},
    position: {x: width/2, y: height/2, z: 0}
  };
  
  // Mouse interaction
  let mouseX = 0, mouseY = 0;
  document.addEventListener('mousemove', (e) => {
    mouseX = e.clientX;
    mouseY = e.clientY;
  });
  
  // Render loop
  function render() {
    ctx.clearRect(0, 0, width, height);
    
    // Update rotation based on mouse position
    cube.rotation.y = (mouseX / width - 0.5) * Math.PI;
    cube.rotation.x = (mouseY / height - 0.5) * Math.PI;
    
    // Project 3D points to 2D
    const projectedPoints = cube.points.map(point => {
      // Rotate point
      const cosX = Math.cos(cube.rotation.x);
      const sinX = Math.sin(cube.rotation.x);
      const cosY = Math.cos(cube.rotation.y);
      const sinY = Math.sin(cube.rotation.y);
      
      const rotatedX = point.x * cos