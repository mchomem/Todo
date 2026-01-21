const originalFetch = window.fetch;

window.fetch = async (input, init) => {
    // Proteção contra sessionStorage vazio
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

    // Inicializar headers se não existir
    init = init || {};
    init.headers = {
        ...(init.headers || {}),
        ...(token ? { Authorization: `Bearer ${token}` } : {})
    };

    try {
        const response = await originalFetch(input, init);

        if (response.status === 401) {
            sessionStorage.removeItem('user'); // Limpar corretamente
            window.location.href = '/pages/login.html';
            return Promise.reject(new Error('Unauthorized'));
        }

        return response;
    } catch (error) {
        throw error;
    }
};