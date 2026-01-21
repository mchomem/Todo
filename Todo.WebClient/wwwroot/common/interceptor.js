const originalFetch = window.fetch;

window.fetch = async (input, init) => {
    // Protection against session. Empty storage.
    const userSession = sessionStorage.getItem('user');
    let token = null;

    if (userSession && userSession !== 'null' && userSession !== '') {
        try {
            const userData = JSON.parse(userSession);
            token = userData?.token;
        } catch (error) {
            console.error('Error parsing user session:', error);
        }
    }

    // Initialize headers if they do not exist
    init = init || {};
    init.headers = {
        ...(init.headers || {}),
        ...(token ? { Authorization: `Bearer ${token}` } : {})
    };

    try {
        const response = await originalFetch(input, init);

        if (response.status === 401) {
            // Clean properly
            sessionStorage.removeItem('user');
            window.location.href = '/pages/login.html';
            return Promise.reject(new Error('Unauthorized'));
        }

        return response;
    } catch (error) {
        throw error;
    }
};