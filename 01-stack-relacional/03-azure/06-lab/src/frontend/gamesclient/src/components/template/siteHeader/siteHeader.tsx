import { useTheme } from 'react-jss';
// import { SiteActions } from './siteActions/siteActions';
import { SiteHeaderStyles } from './siteHeader.jss';

export const SiteHeader = () => {
    const theme = useTheme();
    const styles = SiteHeaderStyles({ theme });

    return (
        <>
            <header className={styles.siteHeader}>
                {/* <SiteActions /> */}
            </header>
        </>
    );
};
