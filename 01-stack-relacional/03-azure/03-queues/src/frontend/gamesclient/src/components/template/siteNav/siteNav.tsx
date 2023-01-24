import { useTheme } from 'react-jss';
import { siteNavData } from '../../../data/siteNav';
import { INavItem } from '../../../models/sitenav/INavItem';
import { SiteNavStyles } from './siteNav.jss';
import { SiteNavItem } from './siteNavItem/siteNavItem';

interface ISiteNavProps {
    menuCollapsed: boolean;
}

export const SiteNav = (props: ISiteNavProps) => {
    const theme = useTheme();
    const styles = SiteNavStyles({ theme });

    const navItems = siteNavData;

    return (
        <>
            <nav className={styles.container}>
                <ul className={styles.options}>
                    {navItems.map((item: INavItem, index: number) => {
                        return (
                            <SiteNavItem
                                navItem={item}
                                key={index}
                            />
                        );
                    })}
                </ul>
            </nav>
        </>
    );
};
