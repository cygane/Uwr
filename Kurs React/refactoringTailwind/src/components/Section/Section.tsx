
interface IProps {
    id: string;
    children: React.ReactNode;
}

export default function Section({ id, children }: IProps) {
    return (
        <section id={id} className={`${'section'} ${id}`}>
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                {children}
            </div>
        </section>
    );
}