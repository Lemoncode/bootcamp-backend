import { createUseStyles } from 'react-jss';

export const SiteNavStyles = createUseStyles((theme: any) => ({
    container: {
        transition: 'all 0.3s',
        background: theme.colors.site.nav.background,
        padding: theme.paddings.site.nav.panel,
    },
    options: {
        display: 'block',
        boxSizing: 'border-box',
        margin: '0',
        padding: '0',
        listStyle: 'none',
        transition: 'all 0.3s',
    },
}));
