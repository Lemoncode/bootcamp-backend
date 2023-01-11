export const palette = {
    white: '#FFFFFF',
    black: '#000000',
    transparent: 'transparent',
    accent: {
        lighter: '#F0F7FE',
        light: '#C9DEF1',
        regular: '#669FCD',
        dark: '#407097',
        darker: '#325169',
        darkest: '#283846',
        accent: '#009FE3',
    },
    gray: {
        lightest: '#F6F5F8',
        lighter: '#EBEBEB',
        light: '#C0C0C0',
        regular: '#A1A1A1',
        dark: '#767676',
        darker: '#5C5C5C',
        darkest: '#333333',
    },
    green: {
        lightest: '#CDF2E0',
        lighter: '#A8E1C5',
        light: '#66C8BB',
        regular: '#36B4A4',
        dark: '',
        darker: '',
        darkest: '',
    },
    red: {
        lightest: '#FFEEEA',
        lighter: '#FFCABD',
        light: '#ED947F',
        regular: '#FD6540',
        dark: '',
        darker: '',
        darkest: '',
    },
};

export const colors = {
    site: {
        background: palette.gray.lightest,
        color: palette.gray.regular,
        corporate: {
            title: palette.gray.dark,
            center: palette.accent.dark,
            mobileBackground: palette.transparent,
            mobileCenter: palette.white,
        },
        mobileHeader: {
            background: palette.accent.dark,
            color: palette.accent.light,
        },
        header: {
            background: palette.accent.dark,
            color: palette.accent.light,
            icon: palette.accent.light,
            mobileIcon: palette.accent.light,
            title: palette.accent.light,
        },
        taskBar: { background: palette.accent.lighter },
        body: {
            background: '',
            color: '',
        },
        content: {
            background: '',
            color: '',
        },
        footer: {
            background: '',
            color: '',
        },
        nav: {
            background: palette.white,
            color: palette.gray.dark,

            item: {
                background: '',
                color: palette.gray.darker,
                hover: {
                    color: palette.accent.dark,
                },
                active: {
                    color: palette.accent.dark,
                    background: palette.gray.lightest,
                },
            },
            configuration: {
                background: palette.gray.lighter,
            },
        },
        mobileNav: {
            background: palette.white,
            color: palette.accent.darkest,
            close: palette.accent.regular,
            hover: {
                color: palette.accent.dark,
                background: palette.accent.light,
            },
            itemActive: {
                background: palette.accent.lighter,
                color: palette.accent.dark,
            },
        },
    },
    titles: {
        section: palette.gray.darkest,
    },
    label: {
        color: palette.gray.regular,
    },
    input: {
        color: palette.gray.darkest,
        border: palette.gray.lighter,
    },
    checkbox: {
        checkedColor: palette.accent.accent,
    },
    button: {
        primary: {
            color: palette.white,
            border: palette.accent.regular,
            background: palette.accent.regular,
        },
        secondary: {
            color: palette.accent.regular,
            border: palette.accent.regular,
            background: palette.transparent,
        },
        terciary: {
            color: palette.accent.regular,
            border: palette.transparent,
            background: palette.transparent,
        },
        success: {
            color: palette.green.lightest,
            border: palette.green.regular,
            background: palette.green.regular,
        },
        danger: {
            color: palette.red.lightest,
            border: palette.red.regular,
            background: palette.red.regular,
            hover: palette.red.light,
        },
        hover: {
            color: palette.accent.dark,
        },
        focus: {
            color: palette.accent.darker,
        },
        disabled: {
            background: palette.accent.lighter,
            color: palette.accent.light,
        },
    },
    spinner: {
        background: palette.gray.regular,
        foreground: palette.accent.regular,
        border: palette.accent.light,
        borderbefore: palette.accent.dark,
        borderafter: palette.accent.regular,
    },
    modal: {
        borderTop: palette.accent.regular,
        icon: palette.accent.accent,
        iconHover: palette.accent.dark,
        iconHoverBackground: palette.accent.lighter,
        buttonBackground: palette.accent.lighter,
    },
    modalInfo: {
        icon: palette.black,
    },
    box: {
        background: palette.white,
        secondaryBackground: palette.accent.lighter,
    },
    form: {
        containerBackground: palette.accent.lighter,
        dataBackground: palette.white,
        buttonBackground: palette.gray.lighter,
        headerBackground: palette.accent.light,
        filesBackground: palette.accent.lighter,
        border: palette.gray.lighter,
        attachedFiles: palette.accent.dark,
        collapsibleSectionTitle: palette.accent.dark,
    },
    home: {
        boxTitle: palette.black,
    },
    profile: {
        informationBody: palette.white,
        informationFooter: palette.gray.lighter,
        navigation: palette.transparent,
        navigationHover: palette.accent.regular,
        fieldContent: palette.gray.darkest,
        fieldName: palette.gray.regular,
    },
    table: {
        header: palette.accent.light,
        headerColor: palette.accent.darker,
        headerHover: palette.accent.lighter,
        cellHover: palette.accent.lighter,
        footer: palette.gray.darkest,
        paginator: palette.white,
        nonRead: palette.accent.regular,
        read: palette.gray.regular,
        regular: palette.gray.darkest,
        noContent: palette.gray.light,
    },
    cards: {
        background: palette.white,
    },
    pivot: {
        active: palette.gray.darkest,
        nonActive: palette.accent.regular,
        borderActive: palette.accent.accent,
    },
    schoolManagement: {
        delete: {
            regular: palette.red.regular,
            hover: palette.red.light,
            focus: palette.red.regular,
        },
        newRegister: {
            color: palette.accent.regular,
            hover: palette.accent.dark,
        },
    },
    messages: {
        background: palette.white,
        actionsBackground: palette.gray.lightest,
        messageBorder: palette.accent.light,
        myConversation: {
            background: palette.accent.dark,
            color: palette.gray.lightest,
            date: palette.accent.light,
            links: palette.accent.regular,
        },
        hisConversation: {
            background: palette.gray.lightest,
            color: palette.gray.darkest,
            date: palette.gray.regular,
            links: palette.accent.regular,
        },
        header: {
            background: palette.accent.lighter,
            color: palette.accent.dark,
            subject: palette.accent.darkest,
        },
    },
    notifications: {
        from: palette.accent.darkest,
        subject: palette.gray.dark,
        relativeTime: palette.gray.light,
        delete: palette.accent.regular,
        border: palette.accent.light,
        title: palette.accent.darkest,
        numberBackground: palette.accent.accent,
        numberColor: palette.white,
    },
    collapsibleCard: {
        title: palette.accent.dark,
        icon: palette.accent.regular,
    },
    permissions: {
        hasPermissionColor: palette.accent.dark,
        title: palette.gray.darkest,
    },
    todayCard: {
        color: palette.gray.darker,
    },
    signin: {
        resume: {
            goToDetail: {
                color: palette.accent.accent,
                background: palette.accent.light,
            },
            signinDataColor: palette.accent.regular,
        },
    },
    tasks: {
        resume: {
            linksColor: palette.accent.accent,
            itemsColor: palette.gray.darker,
        },
    },
};
